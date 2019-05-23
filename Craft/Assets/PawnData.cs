using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnData : MonoBehaviour
{
    public string PawnName {
        get
        {
            return gameObject.name;
        }
    }

    public Sprite PawnSprite
    {
        get
        {
            return gameObject.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public string Description;

    public enum PawnTypes
    {
        Start,
        Battle,
        Split,
        Join,
        Chest
    }

    public PawnTypes PawnType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
