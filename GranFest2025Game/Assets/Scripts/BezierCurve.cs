using System.Collections;
using Unity.VisualScripting;

//using UnityEditor.Search;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class BezierCurve : MonoBehaviour
{
    [SerializeField]
    protected Transform[] points;

    public GameObject carObj;
    
    protected Vector3 targetPosition, targetDirection;
    protected Vector3 lastTargetPosition;

    protected int indexCount;

    [SerializeField] protected float duration;

    protected bool canRun = true;

    protected LineRenderer lineRenderer;
    protected int curveCount;

    [SerializeField]
    protected float speed;
    protected float savedSpeed;

    protected Vector3 lastPos, currentPos;

    protected float speedFactor = 1;


    public bool isObstructed = false;

    protected float savedElapsedTime = 0;

    private float tTime = 0;

    private bool fixedUpdateCanRun = false;

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

    private void Update()
    {

        Debug.DrawRay(carObj.transform.position, carObj.transform.position + targetDirection, Color.green);
        if (fixedUpdateCanRun) {

            float lerpX = Mathf.Lerp(lastTargetPosition.x, targetPosition.x, Time.deltaTime);
            float lerpZ = Mathf.Lerp(lastTargetPosition.z, targetPosition.z, Time.deltaTime);

            carObj.transform.position = new Vector3(lerpX, 0, lerpZ);
            carObj.transform.rotation = Quaternion.LookRotation(targetDirection);

        }
    }

    private void FixedUpdate()
    {
        if (fixedUpdateCanRun)
        {
            if (tTime >= 0.98f)
            {
                var newPos = indexCount + 3;
  

                if (points.Length > newPos + 1 && points.Length > newPos + 3)
                {
                    updateCurvePath();
                }
                else
                {
                    fixedUpdateCanRun = false;
                }
                if (isObstructed)
                {
                    
                    fixedUpdateCanRun = false;

                }
            }
            float speedStep = calSpeed();
            float distPoint = calPointDistance();
            tTime += Time.fixedDeltaTime * speed / distPoint;

            tTime = Mathf.Clamp(tTime, 0, 1);
           
            Vector3 p0 = points[indexCount + 0].position;
            Vector3 p1 = points[indexCount + 1].position;
            Vector3 p2 = points[indexCount + 2].position;
            Vector3 p3 = points[indexCount + 3].position;

            lastTargetPosition = targetPosition;

            targetDirection = calBezCurve(tTime, p0, p1, p2, p3);

            targetPosition =  calBezPoint(tTime, p0, p1, p2, p3);
           
            //carObj.transform.position = targetPosition;
            //carObj.transform.rotation = Quaternion.LookRotation(targetDirection);
        }

    }

    void updateCurvePath()
    {
        indexCount += 3;
        tTime = 0;
    }

    float calPointDistance()
    {
        
        
            float T = (tTime / 4) * 10;
            T = Mathf.FloorToInt(T);

            print("tTime " + tTime);
            print("T " + T);

            var dist = 1.0f;

            switch(T)
            {
                case 0:
                    var p0 = points[indexCount].position;
                    var p1 = points[indexCount + 1].position;
                    dist = Mathf.Sqrt((p1.x - p0.x) * (p1.x - p0.x) + ((p1.z - p0.z) * (p1.z - p0.z)));
                print(dist + " case 0 dist");

                break; 
                case 1:
                    p1 = points[indexCount + 1].position;
                    var p2 = points[indexCount + 2].position;
                    dist = Mathf.Sqrt((p2.x - p1.x) * (p2.x - p1.x) + ((p2.z - p1.z) * (p2.z - p1.z)));
                print(dist + " case 1 dist");
                break; 
                case 2:
                    p2 = points[indexCount + 2].position;
                    var p3 = points[indexCount + 3].position;
                    dist = Mathf.Sqrt((p3.x - p2.x) * (p3.x - p2.x) + ((p3.z - p2.z) * (p3.z - p2.z)));
                print(dist + " case 2 dist");

                break;
                case 3:
                    p3 = points[indexCount + 3].position;
                    var p4 = points[indexCount + 4].position;
                    dist = Mathf.Sqrt((p4.x - p3.x) * (p4.x - p3.x) + ((p4.z - p3.z) * (p4.z - p3.z)));
                print(dist + " case 3 dist");

                break;
            }

        //print("dist " + dist);
        dist *= 10;
       return dist;
       
    }

    public virtual void SetTrack(Transform[] p)
    {
        points = new Transform[p.Length];
        points = p;

        curveCount = (int)(p.Length -1) / 3;

        SetLineRenderer();
       
    }


    public virtual void StartTrack()
    {
        fixedUpdateCanRun = true;
        if (canRun )
        {
            //StartCoroutine(followTrack());
            canRun = false;
        }
    }



    


    private void SetLineRenderer()
    {

        //int linePoints = 10;
        int totalPathCount = (points.Length);
        int totalLinePoints = totalPathCount;
        Vector3[] linepos = new Vector3[totalPathCount];


        for (int j = 0; j < totalPathCount -3; j++)
        {
            Vector3 b0 = points[j + 0].position;
            Vector3 b1 = points[j + 1].position;
            Vector3 b2 = points[j + 2].position;
            Vector3 b3 = points[j + 3].position;

            
            for (int k = 0; k < 4; k++)
            {
                float t = j / (float)(totalPathCount - 1);
                linepos[j + k] = calBezPoint(t, b0, b1, b2, b3);
                //print(linepos[k +k]);

            }
        }

        lineRenderer.positionCount = totalLinePoints;
        lineRenderer.SetPositions(linepos);
    }

    IEnumerator followTrack()
    {
        Vector3 p0 = points[indexCount + 0].position;
        Vector3 p1 = points[indexCount + 1].position;
        Vector3 p2 = points[indexCount + 2].position;
        Vector3 p3 = points[indexCount + 3].position;

      
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

            //carObj.transform.position = lerpPos;
            //carObj.transform.rotation = Quaternion.LookRotation(targetDirection);

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
                else
                {
                    points[points.Length].transform.position = points[0].transform.position; 
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

        float distance = Mathf.Sqrt((lastPos.x - currentPos.x) * (lastPos.x - currentPos.x) + ((lastPos.z - currentPos.z) * (lastPos.z - currentPos.z)));

        float velocity = distance / Time.deltaTime;

        float targetStepSize = speed * Time.deltaTime;
        // 0.002f

       // print("STEP SIZE : " + stepSize);
        if (velocity < targetStepSize)
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
        //print("sum" + sum);
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

