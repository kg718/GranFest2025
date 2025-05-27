using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;

    public GameObject carObj;
    
    private Vector3 targetPosition;

    private int indexCount;



    private void Start()
    {
        //SetTrack(points);
        StartTrack();
    }

    public void SetTrack(Transform[] p)
    {
   
        for (int i = 0; i < p.Length; i++) 
        {
            points[i].position = new Vector3(p[i].position.x, p[i].position.y, p[i].position.z);
        }
       
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

            targetPosition =calBezPoint(lerp, points[0 +indexCount].position, points[1 + indexCount].position, points[2 + indexCount].position, points[3 + indexCount].position);

            yield return new WaitForEndOfFrame();

            if(elapsedTime > duration)
            {
                if (points.Length > indexCount + indexCount)
                {
                    indexCount += 4;
                    StartTrack();
                }
               
            }
                
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

