using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateCounter : MonoBehaviour
{

	[SerializeField] private float _refreshRate = 1f;
	
	private float _frameRate;
	private float _time;
	private int _frames;

	private float _average;
	private int _totalFrames;
	
	private Rect _fpsRect;
	private Rect _avgRect;
	private GUIStyle _style;
	
	void Awake()
	{
		_frameRate = 0f;
		_time = 0f;
		_frames = 0;
		_fpsRect = new Rect(0, Screen.height - 72, 200, 36);
		_avgRect = new Rect(0, Screen.height - 36, 200, 36);
		_style = new GUIStyle() {fontSize = 36};
	}
	// Update is called once per frame
	void Update () {
		if (_time < _refreshRate)
		{
			_time += Time.unscaledDeltaTime;
			_frames++;
		}
		else
		{
			_frameRate = (float) _frames / _time;
			_frames = 0;
			_time = 0;

			_average = CumulativeAverage(_frameRate, _average, _totalFrames);
			_totalFrames++;
		}
	}

	private void OnGUI()
	{
		GUI.Label(_fpsRect, "Frame Rate: " + _frameRate.ToString("F2") + "FPS", _style);
		GUI.Label(_avgRect, "Total Average: " + _average.ToString("F2") + "FPS", _style);
	}

	// Count is count of data in currentAvg, not including new datum
	private static float CumulativeAverage(float datum, float currentAvg, int count)
	{
		return (count * currentAvg + datum) / (count + 1);
	}
}
