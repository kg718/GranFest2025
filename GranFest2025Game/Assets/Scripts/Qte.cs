using UnityEngine;
using UnityEngine.UI;

public class Qte : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        DestroyQTE(4f);
    }

    void Update()
    {
        
    }

    public void UpdateImage(int _sprite)
    {
        image.sprite = sprites[_sprite - 1];
    }

    public void DestroyQTE(float _DestroyTime)
    {
        Destroy(gameObject, _DestroyTime);
    }
}
