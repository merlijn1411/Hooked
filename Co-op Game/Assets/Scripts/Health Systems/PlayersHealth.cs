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

    private void Start()
    {
        for(int i = 0;i < heartsDeadImages.Length;i++)
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
        heartsAliveImages[hearts].enabled = false;
        heartsDeadImages[hearts].enabled = true;

        if (hearts == 0)
            StartCoroutine(EndGame());
        
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
