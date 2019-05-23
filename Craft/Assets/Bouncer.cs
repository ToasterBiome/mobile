using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scale = 1.1f + Mathf.Sin(Time.time * 4) * 0.1f;
        transform.localScale = new Vector3(scale, scale, 1f); 
    }
}
