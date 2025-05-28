using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    [SerializeField] private SpriteRenderer hazRenderer;

    void Start()
    {
        hazRenderer.sprite = sprites[Random.Range(0, sprites.Count)];
    }

    void Update()
    {
        
    }
}
