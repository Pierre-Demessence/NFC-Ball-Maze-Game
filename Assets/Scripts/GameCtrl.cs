using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCtrl : MonoBehaviour
{
	[SerializeField] private AudioSource _music;
	
	[SerializeField] private AudioSource _ballRollingSound;
	
	[SerializeField] private GameObject _panelPause;
	[SerializeField] private GameObject _panelWin;
	[SerializeField] private GameObject _buttonPause;
	
	[SerializeField] private GameObject _ball;
	[SerializeField] private GameObject _ballSpawner;
	
	private int _defaultSleepTimeout;

	private void Awake()
	{
		_defaultSleepTimeout = Screen.sleepTimeout;
	}

	private void Start()
	{
		_music.volume = MainMenu.MusicVolume;
		
		Resume();
	}
	
	private void Pause()
	{
		Screen.sleepTimeout = _defaultSleepTimeout;
		
		_buttonPause.SetActive(false);
		_ballRollingSound.enabled = false;
		
		Time.timeScale = 0;
	}

	private void Resume()
	{
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		
		_buttonPause.SetActive(true);
		_panelPause.SetActive(false);
		_panelWin.SetActive(false);
		_ballRollingSound.enabled = true;
		
		Time.timeScale = 1;
	}

	public void GamePause()
	{		
		_panelPause.SetActive(true);
		
		Pause();
	}

	public void GameResume()
	{
		Resume();
	}

	public void ShowWinScreen()
	{
		_panelWin.SetActive(true);
		
		Pause();
	}
	
	public void PlayAgain()
	{
		_ball.transform.position = _ballSpawner.transform.position;
		
		Resume();
	}
	
	public void BackToMenu()
	{
		SceneManager.LoadScene("MainMenu");
		
		Time.timeScale = 1;
		Screen.sleepTimeout = _defaultSleepTimeout;
	}
}
