using UnityEngine;

public class SpawnHook : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GameObject[] startingPoints;
    [SerializeField] private GameObject hook;

    private void Start()
    {
        for (int i = 0; i < startingPoints.Length; i++)
        {
            GameObject spawnedHook = Instantiate(hook, startingPoints[i].transform.position, Quaternion.identity);

            HookLine hookLine = spawnedHook.GetComponent<HookLine>();

            if (hookLine != null)
            {
                hookLine.SetLineStart(startingPoints[i].transform);
            }
        }
    }
}
