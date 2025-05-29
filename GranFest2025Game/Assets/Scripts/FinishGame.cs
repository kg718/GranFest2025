using TMPro;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public static FinishGame instance;
    private bool hasFinished = false;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TextMeshProUGUI winnerText;
    [SerializeField] private AudioSource finishSound;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        
    }

    public void FinishTheGame(int _winner)
    {
        if(hasFinished)
        {
            return;
        }
        hasFinished = true;
        victoryPanel.SetActive(true);
        switch(_winner)
        {
            case 1:
                winnerText.text = "Winner: Horsey";
                break;
            case 2:
                winnerText.text = "Winner: McFly";
                break;
            case 3:
                winnerText.text = "Winner: VLT Electronic";
                break;
            case 4:
                winnerText.text = "Winner: Red Rabbit Racing";
                break;
        }
    }
}
