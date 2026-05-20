using System;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    public int Id { get; set;}

    [SerializeField] private Sprite lockedImage;
    [SerializeField] private Sprite unlockedImage;
    private Image _imageHolder;

    private void OnDisable()
    {
        _imageHolder = GetComponent<Image>();
    }

    public void isLevelUnlocked(int currentUnlockedLevel)
    {
        _imageHolder.sprite = lockedImage;
        if (Id == currentUnlockedLevel && unlockedImage != null) _imageHolder.sprite = unlockedImage;
    }
}
