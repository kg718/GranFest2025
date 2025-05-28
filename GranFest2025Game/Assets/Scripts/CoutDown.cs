using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class CoutDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    private int number = 3;
    private float currentCountDown = 1f;
    bool started = false;

    public UnityEvent OnGo;

    void Start()
    {

    }

    void Update()
    {
        if (started)
        {
            currentCountDown -= Time.deltaTime;
        }
        if (currentCountDown <= 0f)
        {
            TickDown();
            currentCountDown = 1f;
        }
    }

    public void StartCountDown()
    {
        if (!started)
        {
            countText.gameObject.SetActive(true);
            started = true;
        }
    }

    private void TickDown()
    {
        print("tick");
        number--;
        countText.text = number.ToString();

        if (number == 0)
        {
            countText.text = "GO!!!";
            OnGo.Invoke();
        }
        if (number < 0)
        {
            countText.gameObject.SetActive(false);
            started = false;
        }
    }
}
