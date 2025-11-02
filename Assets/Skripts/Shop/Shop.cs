using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    [SerializeField] private WindowManager _windowManager;

    private PlayerAll _player;

    private void Awake()
    {
        if (_windowManager == null)
            Debug.LogError("WindowManager is NULL!!!");
    }

    public void Interact(PlayerAll player)
    {
        _player = player;
        _player.BlockInput();
        _windowManager.ShopUiEnable(this, player);
    }

    public void EndInteractable()
    {
        _player.UnlockInput();
        _player.CloseInteract();
    }
}
