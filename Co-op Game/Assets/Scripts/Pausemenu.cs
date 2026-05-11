using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Pausemenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;

    [Header("Cursor Settings")]
    [SerializeField] private RectTransform virtualCursor;
    [SerializeField] private float cursorSpeed = 800f;

    private bool isPaused = false;
    private PlayerMovement pausingPlayer;

    public bool IsPaused => isPaused;

    void Start()
    {
        Time.timeScale = 1f;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (virtualCursor != null)
            virtualCursor.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isPaused && virtualCursor != null && pausingPlayer != null)
        {
            float moveX = pausingPlayer.CurrentX;
            float moveY = pausingPlayer.CurrentY;

            Vector2 mapMovement = new Vector2(moveX, moveY) * cursorSpeed * Time.unscaledDeltaTime;
            virtualCursor.anchoredPosition += mapMovement;
        }
    }

    public void PauseGame(PlayerMovement player)
    {
        isPaused = true;
        Time.timeScale = 0f;
        pausingPlayer = player;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(true);

        if (virtualCursor != null)
        {
            virtualCursor.gameObject.SetActive(true);
            virtualCursor.anchoredPosition = Vector2.zero;
        }
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pausingPlayer = null;

        if (pauseMenuPanel != null)
            pauseMenuPanel.SetActive(false);

        if (virtualCursor != null)
            virtualCursor.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }

    public void ClickAtCursor()
    {
        if (virtualCursor == null || EventSystem.current == null) return;

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = virtualCursor.position 
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        if (results.Count > 0)
        {
            foreach (RaycastResult result in results)
            {
                if (result.gameObject == virtualCursor.gameObject)
                {
                    continue;
                }

                Button clickedButton = result.gameObject.GetComponentInParent<Button>();

                if (clickedButton != null && clickedButton.interactable)
                {
                    clickedButton.onClick.Invoke();
                    break; 
                }
            }
        }
    }
}
