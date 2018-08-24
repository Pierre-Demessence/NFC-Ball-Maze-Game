using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	private static float _volumeGlobal = 1f;
	private static float _volumeMusic = 0.5f;
	private static float _volumeSound = 1f;
	
	[SerializeField] private GameObject _mainMenuPanel;
	[SerializeField] private GameObject _settingsPanel;
	
	[SerializeField] private Slider _sliderVolumeGlobal;
	[SerializeField] private Slider _sliderVolumeMusic;
	[SerializeField] private Slider _sliderVolumeSound;
	
	public static float MusicVolume => _volumeMusic * _volumeGlobal;
	public static float SoundVolume => _volumeSound * _volumeGlobal;
	
	[SerializeField] private AudioSource _music;

	private void Awake()
	{
		_volumeGlobal = PlayerPrefs.GetFloat("volumeGlobal", 1f);
		_volumeMusic = PlayerPrefs.GetFloat("volumeMusic", 0.5f);
		_volumeSound = PlayerPrefs.GetFloat("volumeSound", 1f);
	}

	private void Start()
	{
		_settingsPanel.SetActive(false);
		_mainMenuPanel.SetActive(true);
		_music.volume = MusicVolume;
		_sliderVolumeGlobal.value = _volumeGlobal;
		_sliderVolumeMusic.value = _volumeMusic;
		_sliderVolumeSound.value = _volumeSound;
	}

	public void MenuGoPlay()
	{
		SceneManager.LoadScene(1);
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
		Application.Quit();
#endif
	}

	public void SettingsGoMenu()
	{
		_mainMenuPanel.SetActive(true);
		_settingsPanel.SetActive(false);
	}

	public void SettingsGlobalVolumeChanged(float value)
	{
		_volumeGlobal = value;
		_music.volume = MusicVolume;
		PlayerPrefs.SetFloat("volumeGlobal", value);
	}
	
	public void SettingsMusicVolumeChanged(float value)
	{
		_volumeMusic = value;
		_music.volume = MusicVolume;
		PlayerPrefs.SetFloat("volumeMusic", value);
	}
	
	public void SettingsSoundVolumeChanged(float value)
	{
		_volumeSound = value;
		PlayerPrefs.SetFloat("volumeSound", value);
	}
}
