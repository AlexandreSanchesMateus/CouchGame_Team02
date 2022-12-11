using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance { get; private set; }

    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private InputSystemUIInputModule inputModule;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject firstSelected;
    private static bool isGamePaused = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inputModule.enabled = false;
        pauseMenu.alpha = 0;
        pauseMenu.interactable = false;
        pauseMenu.blocksRaycasts = false;
    }

    public void Onpause(InputAction.CallbackContext context)
    {
        if (context.performed) PauseGame();
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            inputModule.enabled = true;
            eventSystem.SetSelectedGameObject(firstSelected);
            Time.timeScale = 0;
            pauseMenu.alpha = 1;
            pauseMenu.interactable = true;
            pauseMenu.blocksRaycasts = true;
        }
        else
        {
            inputModule.enabled = false;
            Time.timeScale = 1;
            pauseMenu.alpha = 0;
            pauseMenu.interactable = false;
            pauseMenu.blocksRaycasts = false;
        }
    }

    public void Resume()
    {
        PauseGame();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        LevelLoader.instance.LoadSceneAnIndex(1);
    }

    public void MainScene()
    {
        Time.timeScale = 1;
        LevelLoader.instance.LoadMenuScene();
    }

    public void Quit()
    {
        Time.timeScale = 1;
        LevelLoader.instance.ExitApplication();
    }
}
