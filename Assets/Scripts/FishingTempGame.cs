using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingTempGame : MonoBehaviour
{
    [Header("Слайдеры")]
    [SerializeField] private Slider redSlider;
    [SerializeField] private Slider greenSlider;
    [SerializeField] private GameObject clickerGameParent;

    [Header("Настройки")]
    [SerializeField] private List<float> possiblePoints = new List<float> { 0.2f, 0.4f, 0.6f, 0.8f };
    [SerializeField] private float successRadius = 0.1f;
    [SerializeField] private float redSliderSpeed = 1f;

    [SerializeField] private ClickFish2Game clickFish2Game;
    [SerializeField] private GameObject parentObject;
    private float targetValue;
    private bool isActive = false;
    private bool movingRight = true;
    private Keyboard keyboard;
    private Mouse mouse;

    public void OnEnable()
    {
        startFishingGame();
    }
    public void startFishingGame()
    {
        keyboard = Keyboard.current;
        mouse = Mouse.current;
        StartMiniGame();
    }
    //private void Start()
    //{
    //    keyboard = Keyboard.current;
    //    mouse = Mouse.current;
    //    StartMiniGame();
    //}

    private void Update()
    {
        if (!isActive) return;

        MoveRedSlider();

        // Проверяем ввод через новую Input System
        if ((mouse != null && mouse.leftButton.wasPressedThisFrame) ||
            (keyboard != null && keyboard.spaceKey.wasPressedThisFrame))
        {
            CheckHit();
        }
    }

    public void StartMiniGame()
    {
        targetValue = possiblePoints[Random.Range(0, possiblePoints.Count)];
        SetupGreenSlider();
        redSlider.value = 0f;
        movingRight = true;
        isActive = true;
    }

    private void SetupGreenSlider()
    {
        greenSlider.value = targetValue;
    }

    private void MoveRedSlider()
    {
        if (movingRight)
        {
            redSlider.value += Time.deltaTime * redSliderSpeed;
            if (redSlider.value >= 1f)
            {
                redSlider.value = 1f;
                movingRight = false;
            }
        }
        else
        {
            redSlider.value -= Time.deltaTime * redSliderSpeed;
            if (redSlider.value <= 0f)
            {
                redSlider.value = 0f;
                movingRight = true;
            }
        }
    }

    private void CheckHit()
    {
        float redSliderValue = redSlider.value;
        float difference = Mathf.Abs(redSliderValue - targetValue);

        if (difference <= successRadius)
        {
            Passed();
        }
        else
        {
            Failed();
        }
    }

    private void Passed()
    {
        Debug.Log($"Успех!");
        isActive = false;
        StartCoroutine(SuccessFeedback());

    }

    private void Failed()
    {
        Debug.Log($"Провал!");
        isActive = false;
        StartCoroutine(FailFeedback());

    }

    private IEnumerator SuccessFeedback()
    {
        Image redImage = redSlider.fillRect.GetComponent<Image>();
        Color originalColor = redImage.color;
        redImage.color = Color.green;
        yield return new WaitForSeconds(0.5f);
        redImage.color = originalColor;
        clickerGameParent.SetActive(true);
        clickFish2Game.StartChallenge();
    }

    private IEnumerator FailFeedback()
    {
        Image redImage = redSlider.fillRect.GetComponent<Image>();
        Color originalColor = redImage.color;
        redImage.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        redImage.color = originalColor;
        CloseGame();
    }

    public void CloseGame()
    {
        parentObject.SetActive(false);
    }
}