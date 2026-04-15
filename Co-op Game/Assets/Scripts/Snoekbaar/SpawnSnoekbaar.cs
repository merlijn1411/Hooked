using UnityEngine;

public class SpawnSnoekbaar : MonoBehaviour
{
    public bool SpawnLeft;

    [Header("Snoekbaar GameObject")]
    [SerializeField] private GameObject snoekbaar;

    public void SpawningSnoekbaar()
    {
        Camera cam = Camera.main;

        SpawnLeft = Random.value > 0.5f;

        float x = SpawnLeft ? -0.1f : 1.1f;
        float y = Random.Range(0f, 0.8f);

        Vector3 viewportPos = new Vector3(x, y, cam.nearClipPlane);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);

        var snoek = Instantiate(snoekbaar, worldPos, Quaternion.identity);
        var snoekScript = snoek.GetComponent<Snoekbaar>();
        snoekScript.SetSnoek(this);
        
        if (!SpawnLeft)
        {
            snoek.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
