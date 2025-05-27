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

    [SerializeField] private float duration;

    private bool canRun = true;



    private void Start()
    {
        //SetTrack(points);
        //StartTrack();
    }

    public void SetTrack(Transform[] p)
    {
        points = new Transform[p.Length];
        points = p;

        //for (int i = 0; i < p.Length; i++) 
        //{
        //    points[i].position = new Vector3(p[i].position.x, p[i].position.y, p[i].position.z);
        //}
       
    }


    public void StartTrack()
    {
        if (canRun)
        {
            StartCoroutine(followTrack());
            canRun = false;
        }
    }



    private void Update()
    {
        carObj.transform.position = targetPosition;

    }




    IEnumerator followTrack()
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            print("aaaa");
            float t = elapsedTime / duration;
            float lerp = Mathf.Lerp(0, 1, t);

            elapsedTime += Time.deltaTime;

            targetPosition =calBezPoint(lerp, points[0 +indexCount].position, points[1 + indexCount].position, points[2 + indexCount].position, points[3 + indexCount].position);

            yield return new WaitForEndOfFrame();

            if(elapsedTime > duration)
            {
                var newPos = indexCount + 3;
                if (points.Length > newPos + 1 && points.Length > newPos + 3)
                {
                    canRun = true;
                    indexCount += 3;
                    StartTrack();
                    yield return new WaitForEndOfFrame();
                    print("sdbvdfuwqa");
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


    private void OnDrawGizmos()
    {
        for (int i = 0; i < points.Length; i++)
        {

            switch(i)
            {
                case 0: Gizmos.color = Color.yellow; break;
                case 1: Gizmos.color = Color.green; break;
                case 2: Gizmos.color = Color.red; break;
                case 3: Gizmos.color = Color.cyan; break;

            }

            Matrix4x4 mat = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(points[i].position, Quaternion.identity, Vector3.one);
            Gizmos.DrawSphere(Vector3.zero, 0.2f);
            Gizmos.matrix = mat;
        }

    }
}

