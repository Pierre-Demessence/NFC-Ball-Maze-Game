﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	private static float _globalVolume = 1f;
	private static float _musicVolume = 0.5f;
	private static float _soundVolume = 1f;
	
	[SerializeField] private GameObject _mainMenuPanel;
	[SerializeField] private GameObject _settingsPanel;
	
	public static float MusicVolume => _musicVolume * _globalVolume;
	public static float SoundVolume => _soundVolume * _globalVolume;
	
	[SerializeField] private AudioSource _music;

	private void Start()
	{
		_settingsPanel.SetActive(false);
		_mainMenuPanel.SetActive(true);
		_music.volume = MusicVolume;
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
