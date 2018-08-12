using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCtrl : MonoBehaviour
{
	[SerializeField] private AudioSource _music;
	
	[SerializeField] private AudioSource _ballRollingSound;
	
	[SerializeField] private GameObject _panelPause;
	[SerializeField] private GameObject _panelWin;
	[SerializeField] private GameObject _buttonPause;
	

	private void Start()
	{
		_music.volume = MainMenu.MusicVolume;
		_buttonPause.SetActive(true);
		_panelPause.SetActive(false);
		_panelWin.SetActive(false);
	}

	public void GamePause()
	{
		_buttonPause.SetActive(false);
		_panelPause.SetActive(true);
		_ballRollingSound.enabled = false;
		Time.timeScale = 0;
	}

	public void GameResume()
	{
		_buttonPause.SetActive(true);
		_panelPause.SetActive(false);
		_ballRollingSound.enabled = true;
		Time.timeScale = 1;
	}

	public void ShowWinScreen()
	{
		_buttonPause.SetActive(false);
		_panelWin.SetActive(true);
	}
	
	public void PlayAgain()
	{
		// TODO 
		
		// _buttonPause.SetActive(true);
		// _panelWin.SetActive(false);
	}
	
	public void BackToMenu()
	{
		_buttonPause.SetActive(true);
		_panelPause.SetActive(false);
		_panelWin.SetActive(false);
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}
}
