using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private BezierCurve curve;
    [SerializeField] private int playerNumber;
    [SerializeField] private float QTESuccessTime;
    [SerializeField] private float CrashTime;
    private float currentQTETime;
    private float currentCrashTime;
    bool canBeHit = true;
    QTEManagement.StopHazardDelegate stopHazard;
    private bool iscrashed = false;
    [SerializeField] private AudioSource crashSFX;
    [SerializeField] private Animator carAnimator;

    private void Start()
    {
        currentQTETime = QTESuccessTime;
        currentCrashTime = CrashTime;
        stopHazard = OnPressQTE;
    }

    private void Update()
    {
        if(currentQTETime > 0)
        {
            currentQTETime -= Time.deltaTime;
        }
        else
        {
            canBeHit = true;
        }
        if (currentCrashTime > 0)
        {
            currentCrashTime -= Time.deltaTime;
        }
        else
        {
            UnCrash();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Warning")
        {
            QTEManagement.Instance.CreateQTE(stopHazard,playerNumber);
        }
        if(other.gameObject.tag == "Hazard" && canBeHit)
        {
            print("crash");
            curve.isObstructed = true;
            currentCrashTime = CrashTime;
            iscrashed = true;
            crashSFX.Play();
            carAnimator.Play("Car_Spin");
        }
        if(other.gameObject.tag == "End")
        {
            FinishGame.instance.FinishTheGame(playerNumber);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Warning")
        {
            
        }
    }

    public void OnPressQTE(int i)
    {
        currentQTETime = QTESuccessTime;
        canBeHit = false;
        UnCrash();
    }

    public void UnCrash()
    {
        if (iscrashed)
        {
            curve.isObstructed = false;
            curve.StartTrack();
            iscrashed = false;
            carAnimator.Play("Car_Idle");
        }
    }
}
