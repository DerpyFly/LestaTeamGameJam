using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Camera _camera;
    public InputActionAsset InputActions;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private InputAction _sprintAction;

    private Vector2 _moveAmt;
    private Vector2 _lookAmt;

    [SerializeField] Rigidbody _rigidbody;

    private float SwimmingSpeed;
    public float Sensetive = 50;
    public float SwimmSpeed = 10;
    public float SprintSpeed = 20;

    private Vector3 _currentVelocity = Vector3.zero;
    public float decelerationRate = 5f;

    public bool _cursorLockMode = true;

    // Новый параметр для максимальной высоты
    public float maxHeight = 10f;
    // Сила отталкивания
    public float pushDownForce = 5f;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _lookAction = InputSystem.actions.FindAction("Look");
        _sprintAction = InputSystem.actions.FindAction("Sprint");

        _rigidbody = GetComponent<Rigidbody>();

    }

    void Update()
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
        _lookAmt = _lookAction.ReadValue<Vector2>();

        if (_sprintAction.IsPressed())
        {
            SwimmingSpeed = SprintSpeed;
        }
        else
        {
            SwimmingSpeed = SwimmSpeed;
        }
    }

    private void FixedUpdate()
    {
        Walking();
        Rotating();
    }

    private void Walking()
    {
        Vector3 targetVelocity = (transform.forward * _moveAmt.x + transform.right * (-_moveAmt.y)) * SwimmingSpeed;
        _currentVelocity = Vector3.Lerp(_currentVelocity, targetVelocity, Time.fixedDeltaTime * decelerationRate);
        _rigidbody.MovePosition(_rigidbody.position + _currentVelocity * Time.fixedDeltaTime);

        if (_currentVelocity.magnitude < 0.1f)
        {
            Quaternion targetRotation = Quaternion.Euler(0, _rigidbody.rotation.eulerAngles.y, 0);
            _rigidbody.rotation = Quaternion.Slerp(_rigidbody.rotation, targetRotation, Time.fixedDeltaTime * decelerationRate);
        }
        else
        {
            Rotating();
        }
    }

    private void Rotating()
    {
        if (_lookAmt != Vector2.zero)
        {
            float rotationAmountY = -_lookAmt.y * Sensetive * Time.deltaTime;
            Quaternion deltaRotationY = Quaternion.Euler(0, 0 , rotationAmountY);
            Quaternion newRotationY = _rigidbody.rotation * deltaRotationY;
            float rotationAmountX = _lookAmt.x * Sensetive * Time.deltaTime;
            Quaternion deltaRotationX = Quaternion.Euler(0, rotationAmountX, 0);
            _rigidbody.MoveRotation(newRotationY * deltaRotationX);
        }
    }

}