using UnityEngine;
using UnityEngine.UI;

public class QTEManagement : MonoBehaviour
{
    public static QTEManagement Instance;

    [SerializeField] private Transform panel;
    [SerializeField] private GameObject QTEButton;
    [SerializeField] private float yMax;

    public delegate void StopHazardDelegate(int PlayerNum);

    void Start()
    {
        Instance = this;
        StopHazardDelegate del = Test;
        //CreateQTE(del, 1);
    }

    public void CreateQTE(StopHazardDelegate _method, int _playerNumber)
    {
        GameObject _newBTN = Instantiate(QTEButton);
        Qte _qte = _newBTN.GetComponent<Qte>();
        _qte.UpdateImage(_playerNumber);
        _newBTN.transform.SetParent(panel);
        float randX = Random.Range(150,1500);
        float randY = Random.Range(120, yMax);
        _newBTN.GetComponent<RectTransform>().position = new Vector2(randX, randY);
        _newBTN.GetComponent<Button>().onClick.AddListener(delegate { _method(_playerNumber); });
    }

    public void Test(int _num)
    {
        print(_num);
    }
}
