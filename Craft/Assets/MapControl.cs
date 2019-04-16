using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float size = Camera.main.orthographicSize - Input.mouseScrollDelta.y * 4;
        size = Mathf.Clamp(size, 2.5f, 10f);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,size,Time.deltaTime * 5f);
    }
}
