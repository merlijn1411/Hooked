using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int Id { get; set;}

    [SerializeField] private Sprite lockedImage;
    [SerializeField] private Sprite unlockedImage;
    [SerializeField] private Image imageHolder;

    public void isLevelUnlocked(int currentUnlockedLevel)
    {
        imageHolder.sprite = lockedImage;
        if (Id == currentUnlockedLevel && unlockedImage != null) imageHolder.sprite = unlockedImage;
    }
}
