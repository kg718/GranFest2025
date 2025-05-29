using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private int playerNumber;
    [SerializeField] private float QTESuccessTime;
    private float currentQTETime;
    bool canBeHit = true;
    QTEManagement.StopHazardDelegate stopHazard;

    private void Start()
    {
        currentQTETime = QTESuccessTime;
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
    }

    public void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if(other.gameObject.tag == "Warning")
        {
            QTEManagement.Instance.CreateQTE(stopHazard,playerNumber);
        }
        if(other.gameObject.tag == "Hazard" && canBeHit)
        {
            
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
        print(i);
        currentQTETime = QTESuccessTime;
        canBeHit = false;
    }
}
