using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private BankUI _bankUI;

    private IInteractable _lastInteractable;

    public void ShopUiEnable(IInteractable interactable, PlayerAll player)
    {
        _shopUI.gameObject.SetActive(true);

        _bankUI.gameObject.SetActive(false);

        _lastInteractable = interactable;
        _shopUI.ViewInventory(player);
        //UI enable;
    }

    public void ShopUiDisable()
    {
        _lastInteractable.EndInteractable();

        _bankUI.gameObject.SetActive(true);

        _shopUI.gameObject.SetActive(false);
    }
}
