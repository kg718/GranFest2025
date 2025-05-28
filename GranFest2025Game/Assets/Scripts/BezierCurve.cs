using System.Collections;
using UnityEditor.Search;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    private Transform[] points;

    public GameObject carObj;
    
    private Vector3 targetPosition, targetDirection;
    private Vector3 lastTargetPosition;

    private int indexCount;

    [SerializeField] private float duration;

    private bool canRun = true;

    private LineRenderer lineRenderer;
    private int curveCount;

    [SerializeField]
    private float speed;
    private float savedSpeed;

    private Vector3 lastPos, currentPos;

    private float speedFactor = 1;


    public bool isObstructed = false;

    private float savedElapsedTime = 0;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //curveCount = (int)(points.Length - 1) / 3;
    }
    private void Start()
    {
        //SetTrack(points);
        savedSpeed = speed;
        //StartTrack();
    }

    public void SetTrack(Transform[] p)
    {
        points = new Transform[p.Length];
        points = p;

        curveCount = (int)(p.Length -1) / 3;
       
    }


    public void StartTrack()
    {
        if (canRun )
        {
            StartCoroutine(followTrack());
            canRun = false;
        }
    }



    private void Update()
    {

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

        float distDependantSpeed = calPointDistance(p0, p1, p2, p3);


        float elapsedTime = 0 + savedElapsedTime;
        while (elapsedTime < 1)
        {
            if (isObstructed)
            {
                savedElapsedTime = elapsedTime;
                canRun = true;
                break;
                
            }

            float t = elapsedTime / 1;


            float lerp = Mathf.Lerp(0, 1, t);

            float speedCal = calSpeed();

            
         
            elapsedTime += Time.deltaTime * speedCal * speed;

            lastTargetPosition = targetPosition;

            targetDirection = calBezCurve(lerp, p0, p1, p2, p3);

            targetPosition = calBezPoint(lerp, p0, p1, p2, p3);

            float lerpX = Mathf.Lerp(lastTargetPosition.x, targetPosition.x, Time.deltaTime);
            float lerpZ = Mathf.Lerp(lastTargetPosition.z, targetPosition.z, Time.deltaTime);

            Vector3 lerpPos = new Vector3(lerpX, 0, lerpZ);

            carObj.transform.position = lerpPos;
            carObj.transform.rotation = Quaternion.LookRotation(targetDirection);

            yield return new WaitForEndOfFrame();

            if(elapsedTime > 1 )
            {
                savedElapsedTime = 0;
                var newPos = indexCount + 3;
                if (points.Length > newPos + 1 && points.Length > newPos + 3)
                {
                    canRun = true;
                    indexCount += 3;
                    StartTrack();
                    yield return new WaitForEndOfFrame();
                }
            }
                
        }
      
    }

    float calSpeed()
    {
       lastPos = currentPos;
        currentPos = carObj.transform.position;

        //print("lastPos : " + lastPos);
        //print("currentPos : " + currentPos);

        float stepSize = Vector3.Magnitude(currentPos - lastPos);
        float targetStepSize = 0.002f;

       // print("STEP SIZE : " + stepSize);
        if (stepSize < targetStepSize)
        {
            speedFactor *= 1.1f;
          //  print("Speed Factor Increase : " + speedFactor);
        }
        else
        {
          //  print("Speed Factor Decrease : " + speedFactor);

            speedFactor *= 0.9f;
        }
        return speedFactor * Time.deltaTime;

  
    }

    float calPointDistance(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        p0 = p0.normalized;
        p1 = p1.normalized;
        p2 = p2.normalized;
        p3 = p3.normalized;

        float d0 = Mathf.Sqrt((p1.x - p0.x) * (p1.x - p0.x) + ((p1.z - p0.z) * (p1.z - p0.z)));
        float d1 = Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + ((p2.z - p1.z) * (p2.z - p1.z)));
        float d2 = Mathf.Sqrt((p3.x - p2.x) * (p3.x - p2.x) + ((p3.z - p2.z) * (p3.z - p2.z)));

        //float d0 = p1.magnitude - p0.magnitude;
        //float d1 = p2.magnitude - p1.magnitude;
        //float d2 = p3.magnitude - p2.magnitude;
        //print("d0 " + d0);
        //print("d1 " + d1);
        //print("d2 " + d2);

        float sum = (d0 + d1 + d2) / 3;
        print("sum" + sum);
        return sum;
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

