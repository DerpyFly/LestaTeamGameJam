using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerEvents))]   
public class PlayerAll : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Bank _bank;

    private Keyboard _keyboard;
    private PlayerEvents _playerEvents;

    private IInteractable _currentInteractable;
    private bool _isInteract = false;

    private bool _isActiveInput = true;

    public Inventory Inventory => _inventory;
    public Bank Bank => _bank;

    private void Awake()
    {
        _playerEvents = GetComponent<PlayerEvents>();
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
            if(_currentInteractable == null || (_currentInteractable != null && ((_keyboard.eKey.wasPressedThisFrame && _inventory.ItemName == ItemName.None) || _keyboard.qKey.wasPressedThisFrame)))
                _inventory.InventoryAction();

            CheckUseInteractable();
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

    public Item PopActiveSlot()
    {
        return _inventory.PopActiveSlot();
    }

    public void ViewWindowInteract(KeyCode keyCode, ItemName notFoundItem = ItemName.None)
    {
        if(keyCode == KeyCode.None)
            _playerEvents.DisableWindowInteract();

        if (notFoundItem == ItemName.None)
            _playerEvents.EnableWindowInteract(keyCode);
        else
            _playerEvents.EnableWindowNotFoundItem(notFoundItem);
    }

    private void CheckUseInteractable()
    {
        if(_currentInteractable != null && !_isInteract && _keyboard.eKey.wasPressedThisFrame)
        {
            _isInteract = true;
            _currentInteractable.Interact(this);
        }
    }
}
