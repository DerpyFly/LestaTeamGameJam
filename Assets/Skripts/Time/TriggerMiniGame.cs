using NUnit.Framework;
using UnityEngine;

public class TriggerMiniGame : MonoBehaviour, IInteractable
{
    [SerializeField] WindowManager _windowManager;
    // [SerializeField] PlayerEvents playerEvents;
    PlayerAll playerAllSave;

    bool isInside = false;
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
        if (!isInside && other.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out PlayerEvents playerEvents))
            {
                playerEvents.OnShowPressHint?.Invoke();
            }
        }

        // if (!isInside && other.gameObject.TryGetComponent(out PlayerAll player))
        // {
        //     if (playerAllSave == null)
        //         playerAllSave = player;

        //     player.ViewWindowInteract(KeyCode.E);
        // }
        isInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (isInside && other.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out PlayerEvents playerEvents))
            {
                other.gameObject.TryGetComponent(out PlayerAll player);
                player.UnlockInput();
                player.CloseInteract();
                playerEvents.OnInteractComplete?.Invoke();
            }
        }

        // if (isInside && other.gameObject.TryGetComponent(out PlayerAll player))
        // {
        //     player.ViewWindowInteract(KeyCode.None);
        // }
        isInside = false;
    }
}
