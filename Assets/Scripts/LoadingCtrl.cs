using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingCtrl : MonoBehaviour
{
    private AsyncOperation _asyncOperation;
    
    [SerializeField] private bool _touchToFinish;
    
    [SerializeField] private Slider _progressBar;
    [SerializeField] private TMP_Text _text;

    // Use this for initialization
    private void Start()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("Game");
        _asyncOperation.allowSceneActivation = !_touchToFinish;
        StartCoroutine(CheckProgress());
    }

    private IEnumerator CheckProgress()
    {
        while (!_asyncOperation.isDone)
        {
            _progressBar.value = _asyncOperation.progress;
            if (_touchToFinish && _asyncOperation.progress >= 0.9f)
            {
                _text.text = "Touch to continue!";
                _progressBar.value = 1f;
                if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
                {
                    _asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }

    }
}