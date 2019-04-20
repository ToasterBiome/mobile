using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootTable
{
    public List<LootItem> table = new List<LootItem>();

    public List<Item> getRandomItem()
    {
        List<Item> drop = new List<Item>();

        int totalWeight = 0;
        foreach(LootItem li in table)
        {
            

            //also add drops that have 100% chance to drop

        if(li.weight == -1)
            {
                drop.Add(li.item);
            } else
            {
                totalWeight += li.weight;
            }
        }

        //generate random number
        int generatedValue = Random.Range(0, totalWeight + 1);

        //add the item

        //walk through the dictionary to find the key

        Debug.Log("Generated value is " + generatedValue);
        Debug.Log("Total Weight value is " + totalWeight);

        int step = 0;
        foreach (LootItem li in table)
        {
            int oldstep = step;
            step += li.weight;

            Debug.Log("Old Step value is " + oldstep);
            Debug.Log("Step value is " + step);

            if (oldstep < generatedValue && step >= generatedValue)
            {
                if(li.item != null)
                {
                    Debug.Log("Adding" + li.item.name + " to drop");
                    drop.Add(li.item);
                    break;
                } else
                {
                    Debug.Log("Hit null");
                    break;
                }
            } 
        }

        return drop;

    }
}
