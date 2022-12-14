using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Timer : MonoBehaviour
{
	public static Timer instance { get; private set; }

	public float timeRemaining, timeValue = 90f;
	public TextMeshPro timerText;
	//public GameObject gameOverScreen;

	private bool timerIsRunning = false;

    private void Start()
    {
		instance = this;
		timeRemaining = timeValue;
		timerIsRunning = true;
    }
    // Update is called once per frame
    void Update()
	{
        if (timerIsRunning)
        {
			if (timeRemaining > 0)
			{
				timeRemaining -= Time.deltaTime;

				DisplayTime(timeRemaining);
			}
			else
			{

				timeRemaining = 0;
				timerIsRunning = false;
				DisplayGameOver();
			}
        }
	}

	public void DisplayTime(float timeToDisplay)
	{
		if (timeToDisplay < 0)
		{
			timeToDisplay = 0;
		}
		//timeToDisplay += 1;
		float minutes = timeToDisplay / 60;
		float secondes = timeToDisplay % 60;
		float milliseconds = (timeToDisplay % 1) * 1000;

		timerText.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, secondes, milliseconds);
	}

	public void DisplayGameOver()
    {
		AudioSpeaker.instance.PauseAudio(true);
		PauseManager.instance.GameOver();
		/*Time.timeScale = 0;
		gameOverScreen.SetActive(true);
		Cursor.lockState = CursorLockMode.Confined;*/
	}

	public void DisplayGame()
    {
		Time.timeScale = 1;
		timeRemaining = timeValue;
		// gameOverScreen.SetActive(false);
		timerIsRunning = true;
		Cursor.lockState = CursorLockMode.Locked;
	}
}
