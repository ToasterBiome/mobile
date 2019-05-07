using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemContainer : MonoBehaviour
{
    public ItemStack itemStack;
    public Text itemNameDesc;
    public RawImage sprite;
    public Text Quantity;
    public GameObject managerParent;

    public void setValues(ItemStack i, string itemNameDescStr, Texture2D _sprite, int quantity, Color tint)
    {
        itemStack = i;
        itemNameDesc.text = itemNameDescStr;
        sprite.texture = _sprite;
        Quantity.text = "x" + quantity;
        sprite.color = tint;
    }

    public void updateQuantity()
    {
        Quantity.text = "x" + itemStack.quantity;
    }

    public void selectItem()
    {
        if(managerParent.GetComponent<InventoryManager>().selectedItem != null)
        {
            managerParent.GetComponent<InventoryManager>().selectedItem.GetComponent<UIItemContainer>().unselectItem();
        }
        
        managerParent.GetComponent<InventoryManager>().selectedItem = gameObject;
        ColorBlock colors = GetComponent<Button>().colors;
        //colors.highlightedColor = new Color(0.78f, 0.78f, 0.78f);

        GetComponent<Button>().colors = colors;

    }

    public void unselectItem()
    {
        ColorBlock colors = GetComponent<Button>().colors;
        //colors.normalColor = Color.white;
        GetComponent<Button>().colors = colors;
    }

    public void Use()
    {
        Debug.Log("used " + itemStack.item.itemName);
        itemStack.item.Use();
        itemStack.quantity -= 1;
        updateQuantity();
        if (itemStack.quantity <= 0)
        {
            BattleManager.instance.player.inventory.Remove(itemStack);
            Destroy(gameObject);
        }
        
    }
}
