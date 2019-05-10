using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    public float TimeToDeath;

    public Vector2 launchVector;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, TimeToDeath);
        launchVector = Random.insideUnitCircle * 8;
    }
	
	// Update is called once per frame
	void Update () {

        
        GetComponent<RectTransform>().localPosition = GetComponent<RectTransform>().localPosition + (Vector3)launchVector;
        
            launchVector *= 0.95f;

        Color textColor = GetComponent<Text>().color;

        //GetComponent<Text>().color = new Color(textColor.r, textColor.g, textColor.b, textColor.a - (1/TimeToDeath));

        //transform.position = new Vector2(transform.position.x, transform.position.y - 4);
    }
}
