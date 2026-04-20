using UnityEngine;

public class SpawnSnoekbaar : MonoBehaviour
{
    public bool SpawnLeft { get; private set; }

    [Header("Snoekbaar GameObject")]
    [SerializeField] private GameObject snoekbaar;

    [Header("Hook Touch Effect")]
    [SerializeField] private PlayersHealth hookTouchEffect;

    public void SpawningSnoekbaar()
    {
        Camera cam = Camera.main;

        SpawnLeft = Random.value > 0.5f;

        float x = SpawnLeft ? -0.15f : 1.15f;
        float y = Random.Range(0f, 0.8f);

        Vector3 viewportPos = new Vector3(x, y, cam.nearClipPlane);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);

        var snoek = Instantiate(snoekbaar, worldPos, Quaternion.identity);
        var snoekScript = snoek.GetComponent<Snoekbaar>();
        var snoekHealth = snoek.GetComponent<HookTouchEffect>();
        snoekScript.SetSnoek(this);
        snoekHealth.SetHealth(hookTouchEffect);

        
        if (!SpawnLeft)
        {
            snoek.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
