using UnityEngine;

public class GameCtrl : MonoBehaviour {

	[SerializeField] private AudioSource _music;

	private void Start ()
	{
		_music.volume = MainMenu.MusicVolume;
	}
	
}
