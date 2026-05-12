using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private Sprite LockedImage;
    [SerializeField] private Sprite UnlockedImage;

    [SerializeField] private bool unlocked = false;

    private Image _imageHolder;
    private void Start()
    {
        _imageHolder = GetComponent<Image>();
        
        CheckLevelsUnlocked();
        
    }

    private void CheckLevelsUnlocked()
    {
        _imageHolder.sprite = LockedImage;
        if (!unlocked) return;
        _imageHolder.sprite = UnlockedImage;
    }

    public void UnlockLevel()
    {
        unlocked = true;
    }


}
