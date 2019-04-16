using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{

    public Vector2[] lastMousePos;

    public Vector3 startPos;
    public Vector3 endPos;

    public bool canAttack = true;
    public bool startedAttacking = false;

    LineRenderer lr;

    public bool successfulAttack = false;
    public bool criticalAttack = false;
    public float distanceMultiplier = 1.0f;

    public Vector3 midPoint;

    public Color weapon_color;

    public float fadeTimer = 0;

    Vector3 getLastPoint()
    {
        if (lr.positionCount > 0)
        {
            return lr.GetPosition(lr.positionCount - 1);
        }
        else
        {
            return new Vector3(0, 0);
        }

    }
    // Use this for initialization
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        //lr.positionCount = 60;
    }

    public void ResetAttack()
    {
        successfulAttack = false;
    }
    public bool isAttackOver()
    {
        if(fadeTimer > 0)
        {
            return false;
        } else
        {
            return true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        successfulAttack = false;
        if (fadeTimer == 0)
        {
            criticalAttack = false;
        }

        if (Input.GetMouseButtonDown(0)) //attempt to attack
        {
            if (canAttack)
            {
                startedAttacking = true;
                startPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            }

        }

        if (Input.GetMouseButtonUp(0)) //ends the attack
        {
            if (startedAttacking)
            {
                //startPos = lr.GetPosition(lr.positionCount - 1);
                endPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
                Vector3 middlePos = Vector3.zero;
                if (lr.positionCount > 0)
                {
                    middlePos = lr.GetPosition((int)lr.positionCount / 2);
                } 
                midPoint = middlePos;
                float hm = startPos.x * (middlePos.y - endPos.y) + middlePos.x * (endPos.y - startPos.y) + endPos.x * (startPos.y - middlePos.y);

                float distance = Vector2.Distance(startPos, endPos);

                if(distance < 1.0f)
                {
                    Debug.Log("Not long enough");
               
                    canAttack = false;
                    startedAttacking = false;
                    
                    return;
                    
                } else
                {
                    distanceMultiplier = Mathf.Clamp(distance, 1, 5);
                    //Debug.Log(distanceMultiplier);
                }

                //Debug.Log(Mathf.Abs(hm));

                if (Mathf.Abs(hm) < 1)
                {
                    if (Mathf.Abs(hm) < 0.25f)
                    {
                        Debug.Log("Critical");
                        criticalAttack = true;
                        successfulAttack = true;
                    } else
                    {
                        Debug.Log("Success");
                        successfulAttack = true;
                    }
                    

                }
                else
                {
                    Debug.Log("Failure");
                }
                canAttack = false;
                startedAttacking = false;

                //Debug.Log(Vector3.Cross(startPos,endPos));
            }

        }

        if (Input.GetMouseButton(0))
        {
            if (canAttack && startedAttacking)
            {


                Vector3 potentialPoint = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);


                if (lr.positionCount < 60)
                {
                    if (Vector3.Distance(potentialPoint, getLastPoint()) > 0.1f)
                    {
                        lr.positionCount += 1;
                        lr.SetPosition(lr.positionCount - 1, potentialPoint);
                    }


                    //lr.positionCount += 1;
                }

            }


            //lr.SetPosition(0, new Vector3(0, 0, 0));
            //lr.SetPosition(lr.positionCount - 1, potentialPoint);
        }



        if (lr.positionCount > 0)
        {
            if (!canAttack)
            {

                fadeTimer += Time.deltaTime / 0.125f;


                //Debug.Log("Fading color");
                if (!criticalAttack)
                {
                    lr.startColor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), fadeTimer);
                    lr.endColor = Color.Lerp(Color.white, new Color(1, 1, 1, 0), fadeTimer);
                }
                else
                {
                    lr.startColor = Color.Lerp(new Color(1, 0.862f, 0.180f), new Color(1, 1, 1, 0), fadeTimer);
                    lr.endColor = Color.Lerp(new Color(1, 0.862f, 0.180f), new Color(1, 1, 1, 0), fadeTimer);
                }

                //Debug.Log(lr.startColor.a);

                if (lr.startColor.a == 0)
                {
                    //Debug.Log("Resetting");
                    lr.positionCount = 0;
                    lr.startColor = Color.white;
                    lr.endColor = Color.white;
                    fadeTimer = 0;
                    canAttack = true;
                }
            }



        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPos, 0.25f);
        Gizmos.DrawLine(startPos, midPoint);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(midPoint, 0.25f);
        Gizmos.DrawLine(midPoint, endPos);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(endPos, 0.25f);
    }

}






