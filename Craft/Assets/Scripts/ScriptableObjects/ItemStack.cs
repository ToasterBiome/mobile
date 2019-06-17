using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int quantity = 0;
    public int slot;

    public ItemStack(Item i, int q)
    {
        item = i;
        quantity = q;
    }

    public ItemStack(Item i, int q, int s)
    {
        item = i;
        quantity = q;
        slot = s;
    }

    public void SetSlot(int s)
    {
        slot = s;
    }

    public bool isEmpty()
    {
        if (item == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setItem(Item i, int q)
    {
        item = i;
        quantity = q;
    }

    public void setItem(ItemStack itemStack)
    {
        item = itemStack.item;
        quantity = itemStack.quantity;
    }

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}
