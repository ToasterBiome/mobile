using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour {

    public Text textComp;
    public Image imageComp;

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
    }

    public void setValues(Vector2 location, float number, int hitType)
    {
        GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        textComp.text = number.ToString();
        GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

        if (hitType == 0)
        {
            textComp.color = Globals.Instance.missHitColor;
            imageComp.color = Globals.Instance.missHitColor;
            textComp.text = "X";
            textComp.fontSize = 48;
            //textComp.GetComponent<Outline>().effectDistance = new Vector2(1, 1);
        }

        if (hitType == 2)
        {
            textComp.color = Globals.Instance.criticalHitColor; //set color to gold
            imageComp.color = Globals.Instance.criticalHitColor;
        }

    }
}
