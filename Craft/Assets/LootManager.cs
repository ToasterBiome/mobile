using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public List<GameObject> items = new List<GameObject>();

    public GameObject itemPrefab;

    public GameObject contentParent;

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void GenerateItems(List<ItemStack> loot)
    {
        Debug.Log("Beginning making items");
        foreach (ItemStack itemStack in loot)
        {
            Debug.Log("Instantiating item object");
            GameObject itemContainer = Instantiate(itemPrefab, contentParent.transform);
            itemContainer.GetComponent<UIItemContainer>().managerParent = gameObject;
            itemContainer.GetComponent<UIItemContainer>().setValues(itemStack, itemStack.item.itemName, itemStack.item.sprite, itemStack.quantity, itemStack.item.color);
            items.Add(itemContainer);
        }
        Debug.Log("Done making items");
    }
}
