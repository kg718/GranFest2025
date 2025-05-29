using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnManagement : MonoBehaviour
{
    public static TurnManagement instance;

    [SerializeField] private TouchControlExample touchControl;
    [SerializeField] private CoutDown countDown;

    private int currentTurn = 1;
    [SerializeField] private BezierCurve[] curves;
    //[SerializeField] private BezierCurve curve;
    [SerializeField] private HazardSpawner hazards;
    [SerializeField] private float playerDrawAmmount;

    [HideInInspector] public float drawAmount1;
    [HideInInspector] public float drawAmount2;
    [HideInInspector] public float drawAmount3;
    [HideInInspector] public float drawAmount4;

    [SerializeField] private Slider drawSlider1;
    [SerializeField] private Slider drawSlider2;
    [SerializeField] private Slider drawSlider3;
    [SerializeField] private Slider drawSlider4;

    [SerializeField] private Color defaultColour = Color.white;
    [SerializeField] private Color yourTurnColour = Color.white;
    [SerializeField] private Color turnOverColour = Color.white;
    [SerializeField] private Image turnImage1;
    [SerializeField] private Image turnImage2;
    [SerializeField] private Image turnImage3;
    [SerializeField] private Image turnImage4;

    [SerializeField] private AudioSource trackMusic;
    [SerializeField] private AudioSource raceMusic;
    [SerializeField] private AudioSource VrrrmmmSFX;

    void Start()
    {
        instance = this;
        drawAmount1 = playerDrawAmmount;
        drawAmount2 = playerDrawAmmount;
        drawAmount3 = playerDrawAmmount;
        drawAmount4 = playerDrawAmmount;
        drawSlider1.maxValue = playerDrawAmmount;
        drawSlider1.value = playerDrawAmmount;
        drawSlider2.maxValue = playerDrawAmmount;
        drawSlider2.value = playerDrawAmmount;
        drawSlider3.maxValue = playerDrawAmmount;
        drawSlider3.value = playerDrawAmmount;
        drawSlider4.maxValue = playerDrawAmmount;
        drawSlider4.value = playerDrawAmmount;
        UpdateTurnColours();
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
                    drawSlider1.value = drawAmount1;
                }
                break;
            case 2:
                if (drawAmount2 >= 0)
                {
                    drawAmount2--;
                    drawSlider2.value = drawAmount2;
                }
                break;
            case 3:
                if (drawAmount3 >= 0)
                {
                    drawAmount3--;
                    drawSlider3.value = drawAmount3;
                }
                break;
            case 4:
                if (drawAmount4 >= 0)
                {
                    drawAmount4--;
                    drawSlider4.value = drawAmount4;
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
        else if(currentTurn == 4)
        {
            countDown.StartCountDown();
            //StartCars();
            hazards.SpawnHazards();
            raceMusic.Play();
            trackMusic.Pause();
        }
        UpdateTurnColours();
    }

    public void UpdateTurnColours()
    {
        switch (currentTurn)
        {
            case 1:
                turnImage1.color = yourTurnColour;
                turnImage2.color = defaultColour;
                turnImage3.color = defaultColour;
                turnImage4.color = defaultColour;
                break;
            case 2:
                turnImage1.color = turnOverColour;
                turnImage2.color = yourTurnColour;
                turnImage3.color = defaultColour;
                turnImage4.color = defaultColour;
                break;
            case 3:
                turnImage1.color = turnOverColour;
                turnImage2.color = turnOverColour;
                turnImage3.color = yourTurnColour;
                turnImage4.color = defaultColour;
                break;
            case 4:
                turnImage1.color = turnOverColour;
                turnImage2.color = turnOverColour;
                turnImage3.color = turnOverColour;
                turnImage4.color = yourTurnColour;
                break;
            default:
                turnImage1.color = defaultColour;
                turnImage2.color = defaultColour;
                turnImage3.color = defaultColour;
                turnImage4.color = defaultColour;
                break;
        }
    }

    public int GetTurn()
    {
        return currentTurn;
    }

    public void StartCars()
    {
        currentTurn++;
        foreach (BezierCurve _curve in curves)
        {
            _curve.StartTrack();
        }
        foreach (Transform t in touchControl.points)
        {
            t.gameObject.GetComponent<LineRenderer>().enabled = false;
        }
        VrrrmmmSFX.Play();
    }
}
