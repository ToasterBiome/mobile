using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject selectedItem;
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

    public void GenerateItems()
    {
        foreach(ItemStack itemStack in BattleManager.instance.player.inventory)
        {
            GameObject itemContainer = Instantiate(itemPrefab, contentParent.transform);
            itemContainer.GetComponent<UIItemContainer>().setValues("<b>" + itemStack.item.itemName + "</b>\n<i>" + itemStack.item.itemDescription + "</i>", itemStack.item.sprite, itemStack.quantity, itemStack.item.color);
            items.Add(itemContainer);
        }
    }
}
