using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable
{
    public List<LootItem> table = new List<LootItem>();

    public List<ItemStack> getRandomItem(int rolls)
    {
        List<ItemStack> drop = new List<ItemStack>();

        
        int totalWeight = 0;
        foreach(LootItem li in table)
        {
            

            //also add drops that have 100% chance to drop

        if(li.weight == -1)
            {
                drop.Add(li.itemStack);
            } else
            {
                totalWeight += li.weight;
            }
        }

        int generatedValue = 0;
        int step = 0;

        for (int i = 0; i < rolls; i++)
        {
            //generate random number
            generatedValue = Random.Range(0, totalWeight + 1);

            //add the item

            //walk through the dictionary to find the key

            Debug.Log("Generated value is " + generatedValue);
            Debug.Log("Total Weight value is " + totalWeight);

            step = 0;
            foreach (LootItem li in table)
            {
                int oldstep = step;
                step += li.weight;

                Debug.Log("Old Step value is " + oldstep);
                Debug.Log("Step value is " + step);

                if (oldstep < generatedValue && step >= generatedValue)
                {
                    if (li.itemStack.item != null)
                    {
                        Debug.Log("Adding" + li.itemStack.item.name + " to drop");
                        //check if it already exists
                        bool alreadyExists = false;
                        for(int j = 0; j < drop.Count; j++)
                        {
                            if (drop[j].item.itemName == li.itemStack.item.itemName)
                            {
                                //it already exists

                                drop[j].quantity += li.itemStack.quantity;
                                alreadyExists = true;
                            }
                        }
                        if(!alreadyExists)
                        {
                            ItemStack itemToAdd = new ItemStack(li.itemStack.item,li.itemStack.quantity);
                            drop.Add(itemToAdd);
                        }
                        
                        break;
                    }
                    else
                    {
                        Debug.Log("Hit null");
                        break;
                    }
                }
            }
        }

        return drop;

    }
}
