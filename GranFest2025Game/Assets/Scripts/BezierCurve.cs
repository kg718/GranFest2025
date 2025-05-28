using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;

    public GameObject carObj;
    
    private Vector3 targetPosition, targetDirection;

    private int indexCount;

    [SerializeField] private float duration;

    private bool canRun = true;

    private LineRenderer lineRenderer;
    private int curveCount;
    

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        curveCount = (int)(points.Length - 1) / 3;
    }
    private void Start()
    {
        //SetTrack(points);
        StartTrack();

       
    }

    public void SetTrack(Transform[] p)
    {
        points = new Transform[p.Length];
        points = p;

        curveCount = (int)(p.Length -1) / 3;
       
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
        carObj.transform.rotation = Quaternion.LookRotation(targetDirection);
        Debug.DrawRay(carObj.transform.position ,carObj.transform.position + targetDirection, Color.green);
       

    }


    

    IEnumerator followTrack()
    {
        Vector3 p0 = points[indexCount + 0].position;
        Vector3 p1 = points[indexCount + 1].position;
        Vector3 p2 = points[indexCount + 2].position;
        Vector3 p3 = points[indexCount + 3].position;

        int linePoints = 30;

        Vector3[] linepos = new Vector3[linePoints];

        for (int i = 0; i < linePoints; i++)
        {
            float t = i / (float)(linePoints - 1);
            linepos[i] = calBezPoint(t, p0, p1, p2, p3);
        }

        lineRenderer.positionCount = linePoints;
        lineRenderer.SetPositions(linepos);

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            print("aaaa");
            float t = elapsedTime / duration;
            float lerp = Mathf.Lerp(0, 1, t);

            elapsedTime += Time.deltaTime;

            targetDirection = calBezCurve(lerp, p0, p1, p2, p3);

            targetPosition = calBezPoint(lerp, p0, p1, p2, p3);

      
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

    Vector3 calBezCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {// this is a tangent 
        float u = 1 - t;

        Vector3 p =
            3 * u * u * (p1 - p0) +
            6 * u * t * (p2 - p1) +
            3 * t * t * (p3 - p2);
        
        return p;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < points.Length; i++)
        {



            Matrix4x4 mat = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(points[i].position, Quaternion.identity, Vector3.one);
            Gizmos.DrawSphere(Vector3.zero, 0.2f);
            Gizmos.matrix = mat;
        }
    
            
    }
}

