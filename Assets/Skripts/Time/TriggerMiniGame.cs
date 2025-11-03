using UnityEngine;

public class TriggerMiniGame : MonoBehaviour, IInteractable
{
    [SerializeField] WindowManager _windowManager;
    public void EndInteractable()
    {
        _windowManager.HudUiEnable();
    }

    public void Interact(PlayerAll playerAll)
    {
        _windowManager.MinigameUiEnable();
    }







}
