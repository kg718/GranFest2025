using UnityEngine;

public class Qte : MonoBehaviour
{

    void Start()
    {
        DestroyQTE(4f);
    }

    void Update()
    {
        
    }

    public void DestroyQTE(float _DestroyTime)
    {
        Destroy(gameObject, _DestroyTime);
    }
}
