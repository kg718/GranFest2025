using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(TouchSimulation))]
public class TouchControlExample : MonoBehaviour
{
    [HideInInspector] public Transform[] points;

    [SerializeField] private BezierCurve curve;
    [SerializeField] private GameObject trackPointPrefab;
    [SerializeField] private float minDist;
    [SerializeField] private float pointStep;
    private float currentPointStep;
    private Transform lastPoint;

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
        GameObject newPoint = Instantiate(trackPointPrefab);
        Vector3 newPos = Camera.main.ScreenToWorldPoint(_touchPoint);
        newPoint.transform.position = new Vector3(newPos.x, 0, newPos.z);
        lastPoint = newPoint.transform;
        currentPointStep = pointStep;
        points = points.Append(newPoint.transform).ToArray();
        TurnManagement.instance.DecrementDrawAmmount();
        curve.SetTrack(points);
    }
}
