using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnData : MonoBehaviour
{
    public GameObject lockGO;

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
    public bool locked = true;

    public enum PawnTypes
    {
        Start,
        Battle,
        Split,
        Join,
        Chest
    }

    public void toggleLock()
    {
        locked = !locked;
        lockGO.SetActive(locked);
    }

    public PawnTypes PawnType;

    // Start is called before the first frame update
    void Start()
    {
        if(!locked)
        {
            lockGO.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
