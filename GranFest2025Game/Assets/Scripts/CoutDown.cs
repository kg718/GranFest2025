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
    [SerializeField] private AudioSource SFX1;
    [SerializeField] private AudioSource SFX2;
    [SerializeField] private AudioSource SFX3;
    [SerializeField] private AudioSource SFXGo;

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
            SFX3.Play();
        }
    }

    private void TickDown()
    {
        number--;
        countText.text = number.ToString();
        switch(number)
        {
            case 3:
                SFX3.Play();
                break;
            case 2:
                SFX2.Play();
                break;
            case 1:
                SFX1.Play();
                break;
        }

        if (number == 0)
        {
            countText.text = "GO!!!";
            OnGo.Invoke();
            SFXGo.Play();
        }
        if (number < 0)
        {
            countText.gameObject.SetActive(false);
            started = false;
        }
    }
}
