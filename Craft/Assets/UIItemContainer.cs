using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemContainer : MonoBehaviour
{
    public Text itemNameDesc;
    public RawImage sprite;
    public Text Quantity;

    // Update is called once per frame
    public void setValues(string itemNameDescStr, Texture2D _sprite, int quantity, Color tint)
    {
        itemNameDesc.text = itemNameDescStr;
        sprite.texture = _sprite;
        Quantity.text = "x" + quantity;
        sprite.color = tint;
    }
}
