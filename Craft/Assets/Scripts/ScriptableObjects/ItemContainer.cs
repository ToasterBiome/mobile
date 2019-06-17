using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemContainer
{
    //ItemContainer is a generic class that has a list of itemSlots to abstract item handling.
    [SerializeField]
    private List<ItemStack> contents = new List<ItemStack>();

    public ItemContainer(int size)
    {
        for(int i = 0; i < size; i++)
        {
            contents.Add(new ItemStack(null, 0, i)); //add empty itemstacks
        }
    }

    public List<ItemStack> GetContents()
    {
        return contents;
    }

    public void setSize(int size)
    {
        if (size > contents.Count)
        {
            for (int i = contents.Count; i < size; i++)
            {
                contents.Add(new ItemStack(null, 0, i)); //add empty itemstacks
            }
        }
        else
        {
            contents.RemoveRange(size, contents.Count - size);
        }
    }

    public ItemStack getSlot(int slot)
    {
        return contents[slot];
    }

    public void addItem(ItemStack itemStack)
    {
        ItemStack occurrence = findFirst(itemStack.item);

        if(occurrence != null) //if it is already in the inventory
        {
            occurrence.quantity += itemStack.quantity;
            return;
        }

        ItemStack firstBlankSpace = findFirst(null);

        if (firstBlankSpace == null)
        {
            Debug.Log("ItemContainer is full");
        }
        else
        {
            firstBlankSpace.setItem(itemStack);
        }
    }

    public void removeItem(Item item, int quantity)
    {
        ItemStack occurrence = findFirst(item);
        if (occurrence == null) return;
        if (occurrence.quantity <= quantity)
        {
            occurrence.Clear();
        }
        else
        {
            occurrence.quantity -= quantity;
        }
    }

    public void clearSlot(int slot)
    {
        contents[slot].item = null;
        contents[slot].quantity = 0;
    }

    public void setItem(Item item, int quantity)
    {
        ItemStack occurrence = findFirst(item);
        if (occurrence == null) return;
        if (quantity == 0)
        {
            occurrence.Clear();
        }
        else
        {
            occurrence.quantity = quantity;
        }
    }

    public ItemStack findFirst(Item item)
    {
        ItemStack first = null;
        for (int i = contents.Count - 1; i >= 0; i--)
        {
            if (contents[i].item == item)
            {
                first = contents[i];
            }
        }
        return first;
    }

    public void AddFromContainer(ItemContainer other)
    {
        foreach(ItemStack stack in other.GetContents())
        {
            addItem(stack);
        }
    }

    public void AddFromList(List<ItemStack> other)
    {
        foreach (ItemStack stack in other)
        {
            addItem(stack);
        }
    }

}
