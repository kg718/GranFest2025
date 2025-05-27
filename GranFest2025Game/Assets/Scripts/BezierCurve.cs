using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    private Transform[] points;

    public GameObject carObj;
    
    private Vector3 targetPosition;



    public void SetPath(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        points[0].position = p0;
        points[1].position = p1;
        points[2].position = p2;
        points[3].position = p3;
    }


    public void StartTrack()
    {
        StartCoroutine(followTrack());
    }



    private void Update()
    {
        carObj.transform.position = targetPosition;
    }




    IEnumerator followTrack()
    {
        float elapsedTime = 0;

        float duration = 5;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float lerp = Mathf.Lerp(0, 1, t);

            elapsedTime += Time.deltaTime;
            targetPosition =calBezPoint(lerp, points[0].position, points[1].position, points[2].position, points[3].position);

            yield return new WaitForEndOfFrame();
        }
      
    }


    Vector3 calBezPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
    
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}

