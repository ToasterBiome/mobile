using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject selectedItem;
    public GameObject selectedItemUI;
    public List<GameObject> items = new List<GameObject>();

    public GameObject itemPrefab;

    public GameObject contentParent;

    private void Start()
    {
        GenerateItems();
    }

    private void Update()
    {
        
    }

    public void refreshSelectedItemUI()
    {
        if(selectedItem != null)
        {
            selectedItemUI.GetComponent<UIItemContainer>().setValues(selectedItem.GetComponent<UIItemContainer>().itemStack);
        } else
        {
            selectedItemUI.GetComponent<UIItemContainer>().Clear(false);
        }
        
    }

    public void UseItem()
    {
        if(selectedItem != null)
        {
            selectedItem.GetComponent<UIItemContainer>().Use();
            //selectedItem = null;
        }
        
    }

    public void GenerateItems()
    {
        foreach(ItemStack itemStack in BattleManager.instance.player.inv.GetContents())
        {
            items[itemStack.slot].GetComponent<UIItemContainer>().setValues(itemStack);
        }
    }
}
