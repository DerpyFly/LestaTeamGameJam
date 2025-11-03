using Unity.Cinemachine;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public PlayerInputActions playerInput;
    [SerializeField] public Rigidbody rb;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float acceleration = 10f;

    [SerializeField] private bool rotateToForwardOnly = true;
    [SerializeField] private float forwardRotateThreshold = 0.1f;
    [SerializeField] private float rotationSmoothTime = 0.12f;

    [SerializeField] private CinemachineCamera _cam;
    [SerializeField] private GameObject SpawnPoint;

    private Vector2 movementInput;
    private Transform cameraTransform;
    private float currentSmoothAngleVelocity;
    private bool hasLastRotation = false;

    void Awake()
    {
        playerInput = new PlayerInputActions();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }



    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        DisablePlayerActionMap();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
        DisablePlayerActionMap();
        _cam.gameObject.SetActive(false);
    }
    public void SpawnPosition()
    {
        rb.gameObject.transform.position = SpawnPoint.transform.position;
        rb.gameObject.transform.rotation = SpawnPoint.transform.rotation;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        Vector3 camForward = cameraTransform != null ? cameraTransform.forward : transform.forward;
        Vector3 camRight = cameraTransform != null ? cameraTransform.right : transform.right;

        Vector3 camForwardHorizontal  = Vector3.ProjectOnPlane(camForward, Vector3.up).normalized;
        Vector3 camRightHorizontal  = Vector3.ProjectOnPlane(camRight, Vector3.up).normalized;

        Vector2 input = movementInput;
        Vector3 desiredMove = Vector3.zero;

        desiredMove += camRightHorizontal * input.x;

        if (input.y > 0.0001f)
        {
            desiredMove += camForward.normalized * input.y;
        }
        else if (input.y < -0.0001f)
        {
            desiredMove += camForwardHorizontal * input.y;
        }

        if (desiredMove.sqrMagnitude > 1f) desiredMove.Normalize();

        Vector3 targetHorizontalVelocity = new Vector3(desiredMove.x * moveSpeed, 0f, desiredMove.z * moveSpeed);

        float targetYVelocity = 0f;

        if (input.y > 0.0001f)
        {
            float camForwardY = camForward.y; 
            if (Mathf.Abs(camForwardY) > 0.01f)
            {
                targetYVelocity = desiredMove.y * moveSpeed;
            }
        }

        Vector3 currentHorizontal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 newHorizontal = Vector3.Lerp(currentHorizontal, targetHorizontalVelocity, Mathf.Clamp01(acceleration * Time.fixedDeltaTime));

        rb.linearVelocity = new Vector3(newHorizontal.x, targetYVelocity, newHorizontal.z);

        if (rotateToForwardOnly && input.y > forwardRotateThreshold && desiredMove.sqrMagnitude > 0.001f)
        {
            Vector3 lookDir = Vector3.ProjectOnPlane(desiredMove, Vector3.up);
            if (lookDir.sqrMagnitude > 0.001f)
            {
                float targetAngle = Mathf.Atan2(lookDir.x, lookDir.z) * Mathf.Rad2Deg;
                if (!hasLastRotation)
                {
                    hasLastRotation = true;
                }

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentSmoothAngleVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }
        }
        else
        {
            if (!hasLastRotation)
            {
                hasLastRotation = true;
            }
        }
    }

    public void EnablePlayerActionMap()
    {
        _cam.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
        playerInput.Player.Move.performed += ctx => movementInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Move.canceled += ctx => movementInput = Vector2.zero;
        playerInput.Player.Enable();
    }

    public void DisablePlayerActionMap()
    {
        _cam.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerInput.Player.Move.performed -= ctx => movementInput = ctx.ReadValue<Vector2>();
        playerInput.Player.Move.canceled -= ctx => movementInput = Vector2.zero;
        playerInput.Player.Disable();
    }
}
