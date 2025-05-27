using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private TouchControlExample touchControl;
    [SerializeField] private GameObject hazard;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SpawnHazards()
    {
        for (int i = 0; i < touchControl.points.Length; i++)
        {
            if(Random.Range(0, 10) == 1)
            {
                Instantiate(hazard, touchControl.points[i].transform.position, Quaternion.identity);
            }
        }
    }
}
