using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayersHealth : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] heartsAliveImages;
    [SerializeField] private Image[] heartsDeadImages;

    [Header("Hearts & damage")]
    [SerializeField] private int damage;
    [SerializeField] private int hearts;

    [Header("Animations")]
    [SerializeField] private Animator loseAnimator;

    [Header("Audio Clip")]
    [SerializeField] private AudioClip lostSound;

    private void Start()
    {
        loseAnimator.enabled = false;
        for (int i = 0;i < heartsDeadImages.Length;i++)
        {
            heartsDeadImages[i].enabled = false;
        }
    }

    public void TakingDamage()
    {
        hearts -= damage;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (hearts <= 0)
        {
            StartCoroutine(EndGame());
            hearts = 0;
        }
            
        heartsAliveImages[hearts].enabled = false;
        heartsDeadImages[hearts].enabled = true;
        
    }

    public bool HasLives()
    {
        var hasLives = hearts > 0;
        return hasLives;
    }

    //This function is called when te player has died and a resart menu can open. For now we just quickly resart the level.
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3);
        SoundManager.Instance.PlaySoundFXClip(lostSound, transform, 1f);
        loseAnimator.enabled = true;
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
