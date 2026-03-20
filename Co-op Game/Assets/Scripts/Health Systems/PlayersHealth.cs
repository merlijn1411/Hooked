using UnityEngine;
using UnityEngine.UI;

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

    void Update()
    {
        TakingDamage();;
    }

    public void TakingDamage()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hearts -= damage;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        switch (hearts)
        {
            case 2:
                heartsAliveImages[0].enabled = false;
                heartsDeadImages[0].enabled = true;
                break;
            case 1:
                heartsAliveImages[1].enabled = false;
                heartsDeadImages[1].enabled = true;
                break;
            case 0:
                heartsAliveImages[2].enabled = false;
                heartsDeadImages[2].enabled = true;
                Debug.Log("You Died");
                //call you died function
                break;
        }
    }
}
