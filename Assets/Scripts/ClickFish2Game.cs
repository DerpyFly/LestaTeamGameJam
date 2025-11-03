using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ClickFish2Game : MonoBehaviour
{

    [SerializeField] private GameObject clickerGameParent;

    [Header("Слайдер")]
    [SerializeField] private Slider verticalSlider;


    [Header("Таймер")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Настройки")]
    [SerializeField] private float challengeDuration = 5f;
    [SerializeField] private int requiredClicks = 15;
    [SerializeField] private float decreaseSpeed = 1f;

    [Header("Визуальные элементы")]
    [SerializeField] private Image sliderFill;
    [SerializeField] private Color successColor = Color.green;
    [SerializeField] private Color failColor = Color.red;
    [SerializeField] private Color normalColor = Color.blue;

    [SerializeField] private FishingTempGame fishingTempGameScript;
    [SerializeField] private Transform rewardSpawnPoint;
    [SerializeField] private List<GameObject> rewardList;
    [SerializeField] private GameObject humanPrefab;

    [SerializeField] private float spawnOffset = 1.5f;
    [SerializeField] private float verticalOffset = 0.5f; // смещение вниз






    private float currentClickValue = 0f;
    private float challengeTimer = 0f;
    private bool isChallengeActive = false;
    private Coroutine decreaseCoroutine;



    // Для новой Input System
    private Mouse mouse;

    private void Start()
    {
        mouse = Mouse.current;
        SetupSlider();
    }

    private void SetupSlider()
    {
        verticalSlider.minValue = 0f;
        verticalSlider.maxValue = 1f;
        verticalSlider.value = 0f;

        if (sliderFill != null)
            sliderFill.color = normalColor;

        if (timerText != null)
            timerText.text = challengeDuration.ToString("F1");


    }

    public void StartChallenge()
    {
        currentClickValue = 0f;
        challengeTimer = challengeDuration;
        isChallengeActive = true;

        verticalSlider.value = 0f;
        if (sliderFill != null)
            sliderFill.color = normalColor;

        UpdateTimerDisplay();

        if (decreaseCoroutine != null)
            StopCoroutine(decreaseCoroutine);

        decreaseCoroutine = StartCoroutine(DecreaseValueOverTime());
        StartCoroutine(ChallengeTimer());

        Debug.Log($"Челлендж начался! Сделайте {requiredClicks} кликов за {challengeDuration} секунд!");
    }

    // Метод для Input System Events (если используешь Events)
    public void OnClick(InputAction.CallbackContext context)
    {
        if (!isChallengeActive) return;

        if (context.performed)
        {
            AddClick();
        }
    }

    private void Update()
    {
        // Используем новую Input System вместо старого Input
        if (isChallengeActive && mouse != null && mouse.leftButton.wasPressedThisFrame)
        {
            AddClick();
        }
    }

    private void AddClick()
    {
        float clickValue = 1f / requiredClicks;
        currentClickValue += clickValue;

        currentClickValue = Mathf.Clamp01(currentClickValue);
        verticalSlider.value = currentClickValue;

        Debug.Log($"Клик! Прогресс: {currentClickValue * 100:F1}%");

        if (currentClickValue >= 1f)
        {
            ChallengeSuccess();
        }
    }

    private IEnumerator DecreaseValueOverTime()
    {
        while (isChallengeActive)
        {
            yield return new WaitForSeconds(0.1f);

            if (currentClickValue > 0f)
            {
                float decreaseAmount = (decreaseSpeed * 0.1f) / challengeDuration;
                currentClickValue = Mathf.Max(0f, currentClickValue - decreaseAmount);
                verticalSlider.value = currentClickValue;

                Debug.Log($"Уменьшение до: {currentClickValue * 100:F1}%");
            }
        }
    }

    private IEnumerator ChallengeTimer()
    {
        while (challengeTimer > 0f && isChallengeActive)
        {
            challengeTimer -= Time.deltaTime;
            UpdateTimerDisplay();

            yield return null;
        }

        if (isChallengeActive)
        {
            if (currentClickValue >= 0.95f)
            {
                ChallengeSuccess();
            }
            else
            {
                ChallengeFailed();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        if (timerText != null)
            timerText.text = challengeTimer.ToString("F1");

        if (timerText != null && challengeTimer < 3f)
        {
            timerText.color = Color.Lerp(Color.red, Color.yellow, challengeTimer / 3f);
        }
    }

    private void ChallengeSuccess()
    {
        isChallengeActive = false;
        if (decreaseCoroutine != null)
            StopCoroutine(decreaseCoroutine);

        if (sliderFill != null)
            sliderFill.color = successColor;

        Debug.Log($"Успех! Прогресс: {currentClickValue * 100:F1}%");

        StartCoroutine(SuccessAnimation());
    }

    private void ChallengeFailed()
    {
        isChallengeActive = false;
        if (decreaseCoroutine != null)
            StopCoroutine(decreaseCoroutine);

        if (sliderFill != null)
            sliderFill.color = failColor;

        Debug.Log($"Провал! Прогресс: {currentClickValue * 100:F1}%");

        StartCoroutine(FailAnimation());
    }

    private IEnumerator SuccessAnimation()
    {
        float pulseTime = 1f;
        float startTime = Time.time;
        Vector3 originalScale = verticalSlider.transform.localScale;

        while (Time.time - startTime < pulseTime)
        {
            float scale = 1f + Mathf.Sin((Time.time - startTime) * 10f) * 0.1f;
            verticalSlider.transform.localScale = originalScale * scale;
            yield return null;
        }

        verticalSlider.transform.localScale = originalScale;
        clickerGameParent.SetActive(false);
        GiveReward();
        fishingTempGameScript.CloseGame();
    }

    private IEnumerator FailAnimation()
    {
        float shakeTime = 0.5f;
        float startTime = Time.time;
        Vector3 originalPos = verticalSlider.transform.localPosition;

        while (Time.time - startTime < shakeTime)
        {
            float shakeX = Random.Range(-10f, 10f);
            float shakeY = Random.Range(-10f, 10f);
            verticalSlider.transform.localPosition = originalPos + new Vector3(shakeX, shakeY, 0);
            yield return null;
        }

        verticalSlider.transform.localPosition = originalPos;
        clickerGameParent.SetActive(false);
        fishingTempGameScript.CloseGame();
    }

    private void GiveReward()
    {
        // Спавним человечка
        Instantiate(humanPrefab, rewardSpawnPoint.position, rewardSpawnPoint.rotation);

        // Получаем 2 случайных номера
        int randomIndex1 = Random.Range(0, rewardList.Count);
        int randomIndex2 = Random.Range(0, rewardList.Count);

        // Спавним предмет слева
        Vector3 leftPosition = rewardSpawnPoint.position + Vector3.left * spawnOffset + Vector3.down * verticalOffset;
        Instantiate(rewardList[randomIndex1], leftPosition, rewardSpawnPoint.rotation);

        // Спавним предмет справа
        Vector3 rightPosition = rewardSpawnPoint.position + Vector3.right * spawnOffset + Vector3.down * verticalOffset;
        Instantiate(rewardList[randomIndex2], rightPosition, rewardSpawnPoint.rotation);
    }
}