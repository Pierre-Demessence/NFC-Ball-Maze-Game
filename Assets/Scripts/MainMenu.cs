using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private static float _globalVolume = 1f;
	private static float _musicVolume = 0.5f;
	private static float _soundVolume = 1f;
	
	[SerializeField] private GameObject _mainMenuPanel;
	[SerializeField] private GameObject _settingsPanel;
	
	[SerializeField] private Slider _sliderVolumeGlobal;
	[SerializeField] private Slider _sliderVolumeMusic;
	[SerializeField] private Slider _sliderVolumeSound;
	
	public static float MusicVolume => _musicVolume * _globalVolume;
	public static float SoundVolume => _soundVolume * _globalVolume;
	
	[SerializeField] private AudioSource _music;

	private void Start()
	{
		_settingsPanel.SetActive(false);
		_mainMenuPanel.SetActive(true);
		_music.volume = MusicVolume;
		_sliderVolumeGlobal.value = _globalVolume;
		_sliderVolumeMusic.value = _musicVolume;
		_sliderVolumeSound.value = _soundVolume;
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
#if UNITY_EDITOR
		Debug.Log("Quitting... Not working in Editor!");
#else
		Application.Quit()
#endif
	}

	public void SettingsGoMenu()
	{
		_mainMenuPanel.SetActive(true);
		_settingsPanel.SetActive(false);
	}

	public void SettingsGlobalVolumeChanged(float value)
	{
		_globalVolume = value;
		_music.volume = MusicVolume;
	}
	
	public void SettingsMusicVolumeChanged(float value)
	{
		_musicVolume = value;
		_music.volume = MusicVolume;
	}
	
	public void SettingsSoundVolumeChanged(float value)
	{
		_soundVolume = value;
	}
}
