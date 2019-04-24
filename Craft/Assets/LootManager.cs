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

    public void GenerateItems(List<Item> loot)
    {
        foreach (Item item in loot)
        {
            GameObject itemContainer = Instantiate(itemPrefab, contentParent.transform);
            itemContainer.GetComponent<UIItemContainer>().setValues(item.itemName, item.sprite, 1, item.color);
            items.Add(itemContainer);
        }
    }
}
