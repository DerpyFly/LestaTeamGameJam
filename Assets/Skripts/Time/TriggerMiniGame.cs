using UnityEngine;

public class TriggerMiniGame : MonoBehaviour, IInteractable
{
    [SerializeField] WindowManager _windowManager;
    PlayerAll playerAllSave;
    public void EndInteractable()
    {
        playerAllSave.UnlockInput();
        playerAllSave.CloseInteract();
        _windowManager.HudUiEnable();
    }

    public void Interact(PlayerAll playerAll)
    {
        playerAllSave = playerAll;
        playerAllSave.BlockInput();
        _windowManager.MinigameUiEnable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerAll player))
        {
            if (playerAllSave == null)
                playerAllSave = player;
            
            player.ViewWindowInteract(KeyCode.E);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerAll player))
        {
            player.ViewWindowInteract(KeyCode.None);
        }
    }
}
