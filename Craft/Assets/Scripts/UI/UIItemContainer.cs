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
        if(itemNameDesc != null)
        {
            itemNameDesc.text = itemNameDescStr;
        }
        sprite.texture = _sprite;
        Quantity.text = "x" + quantity;
        sprite.color = tint;
    }

    public void setValues(ItemStack i)
    {
        itemStack = i;
        if (itemNameDesc != null)
        {
            itemNameDesc.text = "<b>" + itemStack.item.itemName + "</b>\n<i>" + itemStack.item.itemDescription + "</i>";
        }
        sprite.texture = itemStack.item.sprite;
        Quantity.text = "x" + itemStack.quantity;
        sprite.color = itemStack.item.color;
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
        managerParent.GetComponent<InventoryManager>().refreshSelectedItemUI();
    }

    public void unselectItem()
    {
        ColorBlock colors = GetComponent<Button>().colors;
        //colors.normalColor = Color.white;
        GetComponent<Button>().colors = colors;
    }

    public void Clear(bool interactable)
    {
        itemStack = null;
        if(itemNameDesc != null)
        {
            itemNameDesc.text = "";
        }



        sprite.texture = null;
        Quantity.text = null;
        sprite.color = new Color(0f,0f,0f,0f);

        if(interactable)
        {
            GetComponent<Button>().enabled = false;
        }
    }

    public void Use()
    {
        Debug.Log("used " + itemStack.item.itemName);
        itemStack.item.Use();
        itemStack.quantity -= 1;
        updateQuantity();
        if (itemStack.quantity <= 0)
        {
            BattleManager.instance.player.removeFromInventory(itemStack.slot);
            Clear(true);
            GetComponent<Button>().enabled = false;
            //Destroy(gameObject);
            managerParent.GetComponent<InventoryManager>().selectedItem = null;
        }

        managerParent.GetComponent<InventoryManager>().refreshSelectedItemUI();
    }
}
