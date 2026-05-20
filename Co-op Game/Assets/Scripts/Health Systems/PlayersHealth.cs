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

    [Header("Cooldown")]
    [SerializeField] private float invincibilityDuration = 1.5f; // Pas deze tijd naar wens aan in de inspector
    private bool isInvincible = false;

    private void Start()
    {
        for (int i = 0; i < heartsDeadImages.Length; i++)
        {
            heartsDeadImages[i].enabled = false;
        }
    }

    public void TakingDamage()
    {
        if (isInvincible || hearts <= 0) return;

        hearts -= damage;
        UpdateUI();

        if (hearts > 0)
        {
            StartCoroutine(InvincibilityCooldown());
        }
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

    private IEnumerator InvincibilityCooldown()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public bool HasLives()
    {
        var hasLives = hearts > 0;
        return hasLives;
    }

    //This function is called when te player has died and a resart menu can open. For now we just quickly resart the level.
    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
