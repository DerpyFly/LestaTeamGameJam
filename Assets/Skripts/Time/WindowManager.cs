using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private BankUI _bankUI;
    [SerializeField] private FishingController _miniGameUI;


    private IInteractable _lastInteractable;

    public void ShopUiEnable(IInteractable interactable, PlayerAll player)
    {
        _shopUI.gameObject.SetActive(true);

        _miniGameUI.gameObject.SetActive(false);

        _bankUI.gameObject.SetActive(false);

        _lastInteractable = interactable;
        _shopUI.ViewInventory(player, interactable);
        //UI enable;
    }

    public void ShopUiDisable()
    {
        _lastInteractable.EndInteractable();

        _bankUI.gameObject.SetActive(true);

        _miniGameUI.gameObject.SetActive(false);

        _shopUI.gameObject.SetActive(false);
    }

    public void MinigameUiEnable()
    {
        //_lastInteractable.EndInteractable();

        _bankUI.gameObject.SetActive(false);

        _miniGameUI.gameObject.SetActive(true);

        _shopUI.gameObject.SetActive(false);
    }

    public void HudUiEnable()
    {
    }

}
