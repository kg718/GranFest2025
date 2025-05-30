using UnityEngine;

public class HazardSpawner : MonoBehaviour
{
    [SerializeField] private TouchControlExample touchControl;
    [SerializeField] private GameObject hazard;
    [SerializeField] private GameObject end;
    [SerializeField] private int hazardStep;
    [SerializeField, Min(2)] private int hazardChance;

    bool spawned = false;

    public void SpawnHazards()
    {
        if (spawned)
        {
            return;
        }
        Instantiate(end, touchControl.points[touchControl.points.Length - 2].transform.position, Quaternion.identity);
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
            spawned = true;
        }
    }
}
