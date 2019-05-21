using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{

    public bool recording = false;

    public Vector3 startPoint;
    public Vector3 middlePoint;
    public Vector3 endPoint;
    public Vector3 trueMidPoint;

    public int hitType = 0;

    public Color color = Color.white;
    
    public LineRenderer lr;

    public float distance;

    public IEnumerator Fade()
    {
        float time = 0.5f;
        float counter = 0f;
        while(counter < time)
        {
            counter += Time.deltaTime;
            lr.startColor = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), counter/time);
            lr.endColor = Color.Lerp(color, new Color(color.r, color.g, color.b, 0), counter/time);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void startFade()
    {
        StartCoroutine(Fade());
    }
    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void record()
    {
        recording = true;
    }

    public void StopRecord()
    {
        recording = false;

        
    }

    public float[] calculate()
    {
        Vector3 startPos = lr.GetPosition(0);
        Vector3 middlePos = Vector3.zero;
        Vector3 endPos = lr.GetPosition(lr.positionCount - 1);

        if (lr.positionCount > 0)
        {
            middlePos = lr.GetPosition((int)lr.positionCount / 2);
        }

        Vector3 abDiff = middlePos - startPos;
        float abAngle = Mathf.Atan2(abDiff.y, abDiff.x);
        Vector3 bcDiff = endPos - middlePos;
        float bcAngle = Mathf.Atan2(bcDiff.y, bcDiff.x);

        float slopeCalc = abAngle / bcAngle;

        float[] ass = new float[2];
        ass[0] = slopeCalc;
        ass[1] = Vector2.Distance(startPos, endPos);

        distance = ass[1];

        startPoint = startPos;
        middlePoint = middlePos;
        endPoint = endPos;

        trueMidPoint = startPoint + endPoint * 0.5f;

        //lr.Simplify(0.5f);

        if (lr.positionCount == 2)
        {
            lr.positionCount = 3;
            lr.SetPosition(2, lr.GetPosition(1));
            lr.SetPosition(1, (lr.GetPosition(2) + lr.GetPosition(0)) / 2);
        }

        return ass;
    }

    public void betterSimplify()
    {
        lr.Simplify(0.5f);

        if (lr.positionCount == 2)
        {
            lr.positionCount = 3;
            lr.SetPosition(2, lr.GetPosition(1));
            lr.SetPosition(1, (lr.GetPosition(2) + lr.GetPosition(0)) / 2);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!recording)
        {
            return;
        }

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPoint, 0.25f);
        Gizmos.DrawLine(startPoint, middlePoint);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(middlePoint, 0.25f);
        Gizmos.DrawLine(middlePoint, endPoint);
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(endPoint, 0.25f);
    }
}
