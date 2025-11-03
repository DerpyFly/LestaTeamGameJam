
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [Header("Кнопки меню")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;

    [Header("Картинка для показа")]
    public Image introImage;

    [Header("Темный фон")]
    public Image darkBackground;

    [Header("Настройки")]
    public string nextSceneName = "SceneMender";
    public float imageShowTime = 3f;
    public float darkTime = 2f;

    private void Start()
    {
        
        startButton.onClick.AddListener(OnStartClicked);
        quitButton.onClick.AddListener(OnQuitClicked);

        
        if (introImage != null)
        {
            introImage.gameObject.SetActive(false);
        }
        if (darkBackground != null)
        {
            darkBackground.gameObject.SetActive(false);
        }
    }

    private void OnStartClicked()
    {
        
        StartCoroutine(StartGameSequence());
    }

    private IEnumerator StartGameSequence()
    {
        
        startButton.interactable = false;
        if (settingsButton != null) settingsButton.interactable = false;
        quitButton.interactable = false;

        
        if (introImage != null)
        {
            introImage.gameObject.SetActive(true);

            
            float timer = 0f;
            Color color = introImage.color;
            while (timer < 1f)
            {
                color.a = Mathf.Lerp(0f, 1f, timer);
                introImage.color = color;
                timer += Time.deltaTime;
                yield return null;
            }
            introImage.color = new Color(1, 1, 1, 1);

            
            yield return new WaitForSeconds(imageShowTime);
        }

        
        if (darkBackground != null)
        {
            darkBackground.gameObject.SetActive(true);

            
            float timer = 0f;
            Color darkColor = darkBackground.color;
            while (timer < 1f)
            {
                darkColor.a = Mathf.Lerp(0f, 1f, timer);
                darkBackground.color = darkColor;
                timer += Time.deltaTime;
                yield return null;
            }
            darkBackground.color = new Color(0, 0, 0, 1);

            
            yield return new WaitForSeconds(darkTime);
        }

       
        SceneManager.LoadScene(nextSceneName);
    }

    private void OnQuitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}