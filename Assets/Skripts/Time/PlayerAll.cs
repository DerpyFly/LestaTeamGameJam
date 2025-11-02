using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAll : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Bank _bank;

    private Keyboard _keyboard;

    private IInteractable _currentInteractable;
    private bool _isInteract = false;

    private bool _isActiveInput = true;

    public Inventory Inventory => _inventory;
    public Bank Bank => _bank;

    private void Awake()
    {
        _keyboard = Keyboard.current;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            _currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            _currentInteractable = null;
        }
    }

    private void Update()
    {
        if (_isActiveInput) 
        {
            CheckUseInteractable();

            if(_currentInteractable == null)
                _inventory.InventoryAction();
        }
    }

    public void BlockInput()
    {
        _isActiveInput = false;
    }

    public void UnlockInput()
    {
        _isActiveInput = true;
    }

    public void CloseInteract()
    {
        _isInteract = false;
    }

    private void CheckUseInteractable()
    {
        if(_currentInteractable != null && !_isInteract && _keyboard.eKey.wasPressedThisFrame)
        {
            _currentInteractable.Interact(this);
            _isInteract = true;
        }
    }
}
