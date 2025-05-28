using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private TouchControlExample touchControl;
    [SerializeField] private GameObject hazard;
    [SerializeField] private int hazardStep;
    [SerializeField, Min(2)] private int hazardChance;

    public void SpawnHazards()
    {
        for (int i = 0; i < touchControl.points.Length; i++)
        {
            if(i >= touchControl.points.Length - 8)
            {
                return;
            }
            if(Random.Range(0, hazardChance) == 1)
            {
                Instantiate(hazard, touchControl.points[i].transform.position, Quaternion.identity);
                i = i + hazardStep;
            }
        }
    }
}
