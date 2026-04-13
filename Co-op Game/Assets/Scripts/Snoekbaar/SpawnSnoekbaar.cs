using UnityEngine;

public class SpawnSnoekbaar : MonoBehaviour
{
    [SerializeField] private GameObject snoekbaar;

    private void Start()
    {
        SpawningSnoekbaar();
    }

    private void SpawningSnoekbaar()
    {
        Camera cam = Camera.main;

        bool spawnLeft = Random.value > 0.5f;

        float x = spawnLeft ? 0f : 1f;
        float y = Random.Range(0f, 1f);

        Vector3 viewportPos = new Vector3(x, y, cam.nearClipPlane);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);

        var snoek = Instantiate(snoekbaar, worldPos, Quaternion.identity);

        if (!spawnLeft)
        {
            snoek.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
