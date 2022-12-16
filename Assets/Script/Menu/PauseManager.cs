using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance { get; private set; }

    [Header ("General")]
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private InputSystemUIInputModule inputModule;
    [SerializeField] private GameObject background;

    [Header ("Game Over")]
    [SerializeField] private GameObject endGameMenu;
    [SerializeField] private GameObject firstSelected_GO;

    [Header ("Pause")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject firstSelected_PA;
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
        pauseMenu.SetActive(false);
        endGameMenu.SetActive(false);
        background.SetActive(false);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        inputModule.enabled = true;
        endGameMenu.SetActive(true);
        background.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        PlayerControllerProto2.enablePlayerMovement = false;
        eventSystem.SetSelectedGameObject(firstSelected_GO);
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
            Cursor.lockState = CursorLockMode.Confined;
            inputModule.enabled = true;
            eventSystem.SetSelectedGameObject(firstSelected_PA);
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            background.SetActive(true);
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            inputModule.enabled = false;
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
            background.SetActive(false);
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
