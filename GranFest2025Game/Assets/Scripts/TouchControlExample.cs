using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.SceneManagement;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(TouchSimulation))]
public class TouchControlExample : MonoBehaviour
{
    [HideInInspector] public Transform[] points;

    [HideInInspector] public Transform[] pointsP1;
    [HideInInspector] public Transform[] pointsP2;
    [HideInInspector] public Transform[] pointsP3;
    [HideInInspector] public Transform[] pointsP4;

    [SerializeField] private BezierCurve[] curves;
    //[SerializeField] private BezierCurve curve;
    [SerializeField] private GameObject trackPointPrefab;
    [SerializeField] private float minDist;
    [SerializeField] private float pointStep;
    private float currentPointStep;
    private Transform lastPoint;

    [SerializeField] private Vector3[] deadZones;

    void Awake()
    {
        EnhancedTouchSupport.Enable();   
    }

    private void Start()
    {
        currentPointStep = pointStep;
    }

    void Update()
    {
        currentPointStep -= Time.deltaTime;

        if (Touch.activeFingers.Count == 1 && currentPointStep <= 0)
        {
            if(TurnManagement.instance.CheckCanDraw())
            {
                foreach (var touchPoint in Touch.activeTouches)
                {
                    if (lastPoint == null)
                    {
                        DrawPoint(touchPoint.screenPosition);
                    }
                    else if (Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(touchPoint.screenPosition.x, touchPoint.screenPosition.y, 10.0f)), lastPoint.transform.position) > minDist)
                    {
                        DrawPoint(touchPoint.screenPosition);
                    }
                }
            }
        }
        else if(Touch.activeFingers.Count == 0 && !TurnManagement.instance.CheckCanDraw())
        {
            TurnManagement.instance.EndTurn();
        }
    }

    public void DrawPoint(Vector3 _touchPoint)
    {
        //print(_touchPoint);
        for (int i = 0; i < deadZones.Length; i++)
        {
            if (deadZones[i].z == 1)
            {
                if (_touchPoint.x > deadZones[i].x && _touchPoint.y < deadZones[i].y)
                {
                    return;
                }
            }
            else
            {
                if (_touchPoint.x < deadZones[i].x && _touchPoint.y < deadZones[i].y)
                {
                    return;
                }
            }

        }
        GameObject newPoint = Instantiate(trackPointPrefab);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(_touchPoint);
        newPoint.transform.position = new Vector3(newPos.x, 0, newPos.z);
        currentPointStep = pointStep;
        points = points.Append(newPoint.transform).ToArray();
        switch(TurnManagement.instance.GetTurn())
        {
            case 1:
                pointsP1 = points.Append(newPoint.transform).ToArray();
                break;
            case 2:
                pointsP2 = points.Append(newPoint.transform).ToArray();
                break;
            case 3:
                pointsP3 = points.Append(newPoint.transform).ToArray();
                break;
            case 4:
                pointsP3 = points.Append(newPoint.transform).ToArray();
                break;
        }
        LineRenderer line = newPoint.GetComponent<LineRenderer>();
        line.SetPosition(0, newPoint.transform.position);
        if (lastPoint != null)
        {
            line.SetPosition(1, lastPoint.transform.position);
        }
        else
        {
            line.enabled = false;
        }
        lastPoint = newPoint.transform;
        TurnManagement.instance.DecrementDrawAmmount();
        foreach (BezierCurve _curve in curves)
        {
            _curve.SetTrack(points);
        }
        //curve.SetTrack(points);
    }

    public  void OnClickEndTurn()
    {
        TurnManagement.instance.EndTurn();
    }

    public void ResetTrack()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
