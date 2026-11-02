using UnityEngine;

public class BubbelStreamSpawner : MonoBehaviour
{
    [Header("BubbelStream gameobject")]
    [SerializeField] private GameObject bubbelStream;

    [Header("Sounds")]
    [SerializeField] private AudioClip bubbelsSounds;

    public void SpawnBubbelStream()
    {
        Camera cam = Camera.main;

        var SpawnRandom = Random.value > 0.5f;

        float x = Random.Range(0.1f, 0.9f);

        SoundManager.Instance.PlaySoundFXClip(bubbelsSounds, transform, 1f);
        Vector3 viewportPos = new Vector3(x, 0, cam.nearClipPlane);
        Vector3 worldPos = cam.ViewportToWorldPoint(viewportPos);
        Instantiate(bubbelStream, worldPos, Quaternion.identity);
    }
}
