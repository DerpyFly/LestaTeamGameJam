using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndMenuUI : MonoBehaviour
{
    [SerializeField] Button _quitGame;
    [SerializeField] Image _comics;
    [SerializeField] Image _vin;
    [SerializeField] PlayerEvents _playerEvents;

    private float _duration = 1f;
    [SerializeField] Movement _movement;

    void Start()
    {
        _quitGame.onClick.AddListener(OnQuit);
        _vin.gameObject.SetActive(false);
        _quitGame.gameObject.SetActive(false);
        _comics.gameObject.SetActive(false);
        _playerEvents.OnFinalSceneStart.AddListener(StartEndGame);
    }
    
    public void StartEndGame()
    {
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        Debug.Log("START END SCENE");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(VignetteShows(0f));
        yield return new WaitForSeconds(1f);
        _comics.gameObject.SetActive(true);
        yield return StartCoroutine(VignetteShows(1f));
        yield return new WaitForSeconds(6f);
        _quitGame.gameObject.SetActive(true);
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
}
