using UnityEngine;

public class SpawnHook : MonoBehaviour
{  
    [Header("GameObjects")]
    [SerializeField] private GameObject[] startingPoints;
    [SerializeField] private GameObject hook;

    public static SpawnHook instance;   

    private void Start()
    {
        if(instance == null)
        {

        }

        for (var i = 0; i < startingPoints.Length; i++)
        {
            var spawnedHook = Instantiate(hook, startingPoints[i].transform.position, Quaternion.identity);

            var hookLine = spawnedHook.GetComponent<HookLine>();

            if (hookLine != null)
            {
                hookLine.SetLineStart(startingPoints[i].transform);
            }
        }
    }
}
