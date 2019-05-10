using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemStack
{
    public Item item;
    public int quantity;
    public int slot;

    public ItemStack(Item i, int q)
    {
        item = i;
        quantity = q;
    }

    public void SetSlot(int s)
    {
        slot = s;
    }
}
