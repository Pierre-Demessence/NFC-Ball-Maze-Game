using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	[SerializeField] private GameObject _mainMenuPanel;
	[SerializeField] private GameObject _settingsPanel;

	private void Start()
	{
		_settingsPanel.SetActive(false);
		_mainMenuPanel.SetActive(true);
	}

	public void MenuGoPlay()
	{
		SceneManager.LoadScene("Game");
	}

	public void MenuGoSettings()
	{
		_settingsPanel.SetActive(true);
		_mainMenuPanel.SetActive(false);
	}

	public void MenuQuit()
	{
		Application.Quit();	
	}

	public void SettingsGoMenu()
	{
		_mainMenuPanel.SetActive(true);
		_settingsPanel.SetActive(false);
	}
}
