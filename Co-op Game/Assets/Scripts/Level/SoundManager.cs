using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sound Object")]
    [SerializeField] private AudioSource soundFXObject;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float Volume)
    {
        AudioSource audioScource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        audioScource.clip = audioClip;
        audioScource.volume = Volume;

        audioScource.Play();

        var clipLenght = audioScource.clip.length;

        Destroy(audioScource.gameObject, clipLenght);
    }
}
