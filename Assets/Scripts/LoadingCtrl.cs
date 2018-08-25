using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingCtrl : MonoBehaviour
{
    private static readonly string[] _loadingRandomMessages =
    {
        "Reticulating Splines...",
        "Polishing the edges...",
        "Adding some qubits...",
        "Adding gravity...",
        "Extruding the meshes..."
    };
    private AsyncOperation _asyncOperation;
    [SerializeField] private TMP_Text _loadingMessage;

    [SerializeField] private Slider _progressBar;

    [SerializeField] private bool _touchToFinish;

    private void Start()
    {
        _asyncOperation = SceneManager.LoadSceneAsync("Game");
        _asyncOperation.allowSceneActivation = !_touchToFinish;
        StartCoroutine(CheckProgress());
        StartCoroutine(ChangeText());
    }

    private IEnumerator CheckProgress()
    {
        while (!_asyncOperation.isDone)
        {
            _progressBar.value = _asyncOperation.progress;
            if (_touchToFinish && _asyncOperation.progress >= 0.9f)
            {
                _loadingMessage.text = "Touch to continue!";
                _progressBar.value = 1f;
                if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) _asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    private IEnumerator ChangeText()
    {
        while (!_asyncOperation.isDone)
        {
            _loadingMessage.text = _loadingRandomMessages.OrderBy(a => Guid.NewGuid()).First();
            yield return new WaitForSecondsRealtime(Random.value + 2);
        }
    }
}