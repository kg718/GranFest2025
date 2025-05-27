using UnityEngine;

public class TurnManagement : MonoBehaviour
{
    public static TurnManagement instance;

    private int currentTurn = 1;
    [SerializeField] private BezierCurve curve;
    [SerializeField] private HazardSpawner hazards;
    [SerializeField] private float playerDrawAmmount;

    [HideInInspector] public float drawAmount1;
    [HideInInspector] public float drawAmount2;
    [HideInInspector] public float drawAmount3;
    [HideInInspector] public float drawAmount4;

    void Start()
    {
        instance = this;
        drawAmount1 = playerDrawAmmount;
        drawAmount2 = playerDrawAmmount;
        drawAmount3 = playerDrawAmmount;
        drawAmount4 = playerDrawAmmount;
    }

    void Update()
    {
        
    }

    public void DecrementDrawAmmount()
    {
        switch (currentTurn)
        {
            case 1:
                if (drawAmount1 >= 0)
                {
                    drawAmount1--;
                }
                break;
            case 2:
                if (drawAmount2 >= 0)
                {
                    drawAmount2--;
                }
                break;
            case 3:
                if (drawAmount3 >= 0)
                {
                    drawAmount3--;
                }
                break;
            case 4:
                if (drawAmount4 >= 0)
                {
                    drawAmount4--;
                }
                break;
        }
    }

    public bool CheckCanDraw()
    {
        switch (currentTurn)
        {
            case 1:
                if(drawAmount1 <= 0)
                {
                    return false;
                }
                break;
            case 2:
                if (drawAmount2 <= 0)
                {
                    return false;
                }
                break;
            case 3:
                if (drawAmount3 <= 0)
                {
                    return false;
                }
                break;
            case 4:
                if (drawAmount4 <= 0)
                {
                    return false;
                }
                break;
            default:
                return false;
        }
        return true;
    }

    public void EndTurn()
    {
        if(currentTurn <= 3)
        {
            currentTurn++;
        }
        else if(currentTurn ==4)
        {
            currentTurn++;
            curve.StartTrack();
            hazards.SpawnHazards();
        }
    }
}
