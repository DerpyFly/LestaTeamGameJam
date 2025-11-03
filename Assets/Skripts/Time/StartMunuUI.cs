using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] Button _startGame;
    [SerializeField] Button _quitGame;
    [SerializeField] Image _comics;
    [SerializeField] Image _vin;

    private float _duration = 1f;
    [SerializeField] Movement _movement;

    void Start()
    {
        _comics.gameObject.SetActive(false);
        _vin.gameObject.SetActive(false);
        _startGame.onClick.AddListener(OnStart);
        _quitGame.onClick.AddListener(OnQuit);
    }

    private void OnStart()
    {
        _startGame.gameObject.SetActive(false);
        _quitGame.gameObject.SetActive(false);
        _comics.gameObject.SetActive(true);
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        yield return new WaitForSeconds(3f);
        yield return StartCoroutine(VignetteShows(0f));
        yield return new WaitForSeconds(1f);
        Go(); 
        yield return StartCoroutine(VignetteShows(1f));
    }

    private void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator VignetteShows(float startcolor)
    {
        float elapsedTime = 0f;
        Color color = _vin.color;
        color.a = startcolor;
        _vin.color = color;
        _vin.gameObject.SetActive(true);
        float targetAlpha = startcolor == 0f ? 1f : 0f;

        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startcolor, targetAlpha, elapsedTime / _duration);
            _vin.color = color;
            yield return null;
        }


        color.a = targetAlpha;
        _vin.color = color;
    }

    private void Go()
    {
        _comics.gameObject.SetActive(false);
        _movement.SpawnPosition();
        _movement.EnablePlayerActionMap();
    }
}
