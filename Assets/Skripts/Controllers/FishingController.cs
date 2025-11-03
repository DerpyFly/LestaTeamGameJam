using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;


public class FishingController : MonoBehaviour
{
    public InputActionAsset InputActions;
    private InputAction _catchAction;
    private InputAction _pullAction;

    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] Button _exitButton;
    [SerializeField] Button _restartButton;
    [SerializeField] Slider _slider;
    [SerializeField] TextMeshProUGUI _timer;

    [SerializeField] GameObject _background;
    [SerializeField] GameObject _greenzone;
    [SerializeField] GameObject _redzone;

    [SerializeField] float _speed = 200.0f;
    [SerializeField] float _length = 250.0f;

    [SerializeField] float _timerDuration = 3f; 
    [SerializeField] float _requiredClicks = 5;
    [SerializeField] float _downgradeSlider = 1;

    private Vector3 _startPos;
    private bool _movingRight = true;
    private bool _canMove = true;
    private RectTransform _redzoneRectTransform;
    private RectTransform _greenzoneRectTransform;

    private bool gamestart;
    private float _clickCount = 0;

    private PlayerAll _player;


    private void OnEnable()
    {
        InputActions.FindActionMap("UI").Enable();
        _catchAction = InputActions.FindAction("Catch");
        _pullAction = InputActions.FindAction("Pull");
        _catchAction.performed += OnCatch;

        _exitButton.onClick.AddListener(OnExitButtonClick);
        _restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnDisable()
    {
        _catchAction.performed -= OnCatch;

        _exitButton.onClick.RemoveListener(OnExitButtonClick);
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);
    }

    void Start()
    {
        _redzoneRectTransform = _redzone.GetComponent<RectTransform>();
        _greenzoneRectTransform = _greenzone.GetComponent<RectTransform>();
        _startPos = _redzoneRectTransform.anchoredPosition;
        _textMeshPro.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(false);
        _slider.gameObject.SetActive(false);
        _timer.gameObject.SetActive(false);
        StartGame();
    }

    private void StartGame()
    {
        _pullAction.performed -= OnPull;
        float greenzoneWidth = _greenzoneRectTransform.rect.width;
        float randomXPosition = Random.Range(-_length + (greenzoneWidth / 2), _length - (greenzoneWidth / 2));
        _greenzoneRectTransform.anchoredPosition = new Vector2(randomXPosition, _greenzoneRectTransform.anchoredPosition.y);
        _textMeshPro.gameObject.SetActive(false);
        _greenzone.SetActive(true);
        _restartButton.gameObject.SetActive(false);
        _slider.gameObject.SetActive(false);
        _slider.value = 0;
        gamestart = true;
        _clickCount = 0;
        _canMove = true;
        _redzoneRectTransform.anchoredPosition = _startPos;
    }

    void Update()
    {
        if (gamestart)
        {
            RedZoneMove();
        }
    }

    private void RedZoneMove()
    {
        if (_canMove)
        {
            float step = _speed * Time.deltaTime;
            if (_movingRight)
            {
                _redzoneRectTransform.anchoredPosition += new Vector2(step, 0);
            }
            else
            {
                _redzoneRectTransform.anchoredPosition -= new Vector2(step, 0);
            }
            if (Vector2.Distance(_redzoneRectTransform.anchoredPosition, _startPos) >= _length)
            {
                _movingRight = !_movingRight;
            }
        }
    }

    private void OnCatch(InputAction.CallbackContext context)
    {
        _canMove = false;
        if (IsRedZoneInGreenZone())
        {
            ShowText("Клюнул");
            _pullAction.performed += OnPull;
            StartTimer();
        }
        else
        {
            ShowText("Не поймал");
            _restartButton.gameObject.SetActive(true);
        }
    }


    private void OnPull(InputAction.CallbackContext context)
    {
        if (_clickCount < _requiredClicks)
        {
            _clickCount++;
            _slider.value = (float)_clickCount / _requiredClicks;

            if (_clickCount >= _requiredClicks)
            {
                _pullAction.performed -= OnPull; 
            }
        }
    }

    private void ShowText(string message)
    {
        _textMeshPro.text = message; 
        _textMeshPro.gameObject.SetActive(true); 
    }

    private bool IsRedZoneInGreenZone()
    {
        Vector3[] greenZoneCorners = new Vector3[4];
        _greenzoneRectTransform.GetWorldCorners(greenZoneCorners);

        Vector3[] redZoneCorners = new Vector3[4];
        _redzoneRectTransform.GetWorldCorners(redZoneCorners);

        return (redZoneCorners[0].x >= greenZoneCorners[0].x &&
                redZoneCorners[0].y <= greenZoneCorners[1].y &&
                redZoneCorners[2].x <= greenZoneCorners[2].x &&
                redZoneCorners[2].y >= greenZoneCorners[0].y);
    }

    private void StartTimer()
    {
        _slider.gameObject.SetActive(true);
        _timer.gameObject.SetActive(true);
        StartCoroutine(TimerCoroutine());
    }

    private System.Collections.IEnumerator TimerCoroutine()
    {
        float timer = _timerDuration;
        float elapsedTime = 0f;
        _timer.text = FormatTime(timer);

        float sliderValue = _slider.value;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            elapsedTime += Time.deltaTime;

            _timer.text = FormatTime(timer);

            // Уменьшение времени
            if (elapsedTime >= 0.1f)
            {

                _clickCount = Mathf.Max(0f, _clickCount - (_downgradeSlider*0.1f)); 
                _slider.value = _clickCount / _requiredClicks; 
                elapsedTime = 0f;
            }

            yield return null;

            // Проверка заполнения слайдера
            if (_clickCount >= _requiredClicks)
            {
                ShowText("Поймал!!!");
                _timer.gameObject.SetActive(false);
                _restartButton.gameObject.SetActive(true);
                _slider.gameObject.SetActive(false);
                yield break;
            }
        }

        if (_clickCount < _requiredClicks)
        {
            ShowText("Сорвалась");
        }

        _restartButton.gameObject.SetActive(true);
        _slider.gameObject.SetActive(false);
        _timer.gameObject.SetActive(false);
    }

    private string FormatTime(float time)
    {
        int seconds = Mathf.FloorToInt(time);
        int milliseconds = Mathf.FloorToInt((time - seconds) * 1000); 
        return $"{seconds:0}:{milliseconds:00}";
    }

    private void OnExitButtonClick()
    {
        Application.Quit();
    }

    private void OnRestartButtonClick()
    {
        StartGame(); 
    }
}