using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashContainer : MonoBehaviour
{
    public GameObject slashObject;

    public GameObject currentSlash;

    public bool inUse = false;

    public bool successfulAttack = false;
    public bool criticalAttack = false;
    public Vector3 slashMidPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        successfulAttack = false;
        criticalAttack = false;

        if(Input.GetMouseButtonDown(0))
        {
            //create new slash and start recording
            currentSlash = Instantiate(slashObject,Vector3.zero,Quaternion.identity);
            currentSlash.GetComponent<Slash>().record();
            inUse = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //stop slash, do calculations, start fade
            currentSlash.GetComponent<Slash>().StopRecord();

            float [] results = currentSlash.GetComponent<Slash>().calculate();

            float precision = results[0];
            if (Mathf.Abs(precision - 1) < 0.25f)
            {
                //succesful hit
                successfulAttack = true;
                //currentSlash.GetComponent<Slash>().betterSimplify();
                if (Mathf.Abs(precision - 1) < 0.05f)
                {
                    currentSlash.GetComponent<Slash>().hitType = 2;
                    criticalAttack = true;
                    currentSlash.GetComponent<Slash>().color = new Color(1, 0.862f, 0.180f);
                }
                else
                {
                    currentSlash.GetComponent<Slash>().hitType = 1;
                }
            } else
            {
                currentSlash.GetComponent<Slash>().hitType = 0;
                currentSlash.GetComponent<Slash>().color = new Color(.5f, .5f, .5f);
            }

            if (currentSlash.GetComponent<Slash>().distance < 3)
            {
                currentSlash.GetComponent<Slash>().hitType = 0;
                currentSlash.GetComponent<Slash>().color = new Color(.5f, .5f, .5f);
            }


            BattleManager.instance.recieveHit(currentSlash);

            currentSlash.GetComponent<Slash>().startFade();
            slashMidPos = currentSlash.GetComponent<Slash>().middlePoint;
            
            currentSlash = null;
            inUse = false;
        }
    }
}
