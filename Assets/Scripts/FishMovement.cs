using UnityEngine;
using UnityEngine.InputSystem;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float accelerationSpeed = 8f;
    public float decelerationSpeed = 6f;
    public float maxSpeed = 15f;
    public float minSpeed = 2f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 2f;
    public float maxPitchAngle = 80f;

    private float yaw = 0f;
    private float pitch = 0f;
    private Vector3 movementDirection;
    private float currentSpeed;

    // Input variables
    private Vector2 mouseDelta;
    private float accelerationInput;

    // Input Actions
    private PlayerInput playerInput;
    private InputAction lookAction;
    private InputAction accelerateAction;
    private InputAction brakeAction;

    void Start()
    {
        currentSpeed = moveSpeed;

        // Инициализация Input System
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            playerInput = gameObject.AddComponent<PlayerInput>();
        }

        // Настройка Input Actions
        lookAction = playerInput.actions["Look"];
        accelerateAction = playerInput.actions["Accelerate"];
        brakeAction = playerInput.actions["Brake"];

        // Блокируем и скрываем курсор
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleInput();
        HandleMouseLook();
        HandleAcceleration();
        HandleMovement();
    }

    void HandleInput()
    {
        // Получаем ввод от мыши
        mouseDelta = lookAction.ReadValue<Vector2>();

        // Получаем ввод ускорения/торможения
        float accelerate = accelerateAction.ReadValue<float>();
        float brake = brakeAction.ReadValue<float>();

        // Комбинируем ввод ускорения и торможения
        accelerationInput = accelerate - brake;
    }

    void HandleMouseLook()
    {
        // Применяем ввод мыши
        yaw += mouseDelta.x * mouseSensitivity * Time.deltaTime;
        pitch -= mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // Ограничиваем угол наклона
        pitch = Mathf.Clamp(pitch, -maxPitchAngle, maxPitchAngle);

        // Применяем поворот только по Yaw (горизонталь) для рыбы
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);
    }

    void HandleAcceleration()
    {
        // Ускорение
        if (accelerationInput > 0)
        {
            currentSpeed += accelerationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        }
        // Торможение
        else if (accelerationInput < 0)
        {
            currentSpeed -= decelerationSpeed * Time.deltaTime;
            currentSpeed = Mathf.Max(currentSpeed, minSpeed);
        }
        // Плавное возвращение к базовой скорости
        else
        {
            if (currentSpeed > moveSpeed)
            {
                currentSpeed -= decelerationSpeed * Time.deltaTime;
                currentSpeed = Mathf.Max(currentSpeed, moveSpeed);
            }
            else if (currentSpeed < moveSpeed)
            {
                currentSpeed += accelerationSpeed * Time.deltaTime;
                currentSpeed = Mathf.Min(currentSpeed, moveSpeed);
            }
        }
    }

    void HandleMovement()
    {
        // Движение всегда вперед по локальной оси Z
        movementDirection = transform.forward;

        // Двигаемся вперед с текущей скоростью
        transform.Translate(movementDirection * currentSpeed * Time.deltaTime, Space.World);
    }

    // Для отображения текущей скорости в инспекторе
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        GUI.Label(new Rect(10, 10, 300, 30), $"Speed: {currentSpeed:F1}", style);
        GUI.Label(new Rect(10, 40, 300, 30), $"Acceleration Input: {accelerationInput:F1}", style);
    }

    // Для отладки - разблокировать курсор по нажатию Escape
    void LateUpdate()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}