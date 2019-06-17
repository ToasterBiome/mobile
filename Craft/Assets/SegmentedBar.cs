using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedBar : MonoBehaviour
{

    public List<GameObject> segments = new List<GameObject>();

    private int _value;
    [SerializeField]
    public int value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
            for (int i = 0; i < segments.Count; i++)
            {
                if(i < _value)
                {
                    segments[i].GetComponent<Image>().color = Color.red;
                } else
                {
                    segments[i].GetComponent<Image>().color = Color.black;
                }
                
            }
        }
    }

    void Awake()
    {
        value = segments.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
