using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private ShopUI _shopUI;
    [SerializeField] private BankUI _bankUI;
    [SerializeField] private FishingController _miniGameUI;
    [SerializeField] private GameObject _showBtnHintUI;
    [SerializeField] private GameObject _showMissingItemtUI;

    private IInteractable _lastInteractable;
    private PlayerEvents playerEvents;

    void Start()
    {
        playerEvents = FindAnyObjectByType<PlayerEvents>();
        playerEvents.OnShowPressHint.AddListener(BtnHintUiEnable);
        playerEvents.OnInteractComplete.AddListener(BtnHintUiDisable);

        _showBtnHintUI.SetActive(false);
        _showMissingItemtUI.SetActive(false);
    }

    void OnDisable()
    {
        playerEvents.OnShowPressHint.RemoveAllListeners();
        playerEvents.OnInteractComplete.RemoveAllListeners();
    }

    public void ShopUiEnable(IInteractable interactable, PlayerAll player)
    {
        _shopUI.gameObject.SetActive(true);

        _miniGameUI.gameObject.SetActive(false);

        _bankUI.gameObject.SetActive(false);

        _lastInteractable = interactable;
        _shopUI.ViewInventory(player, interactable);

        BtnHintUiDisable();
        
        MissingItemtUiDisable();
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

        BtnHintUiDisable();

        MissingItemtUiDisable();
    }
    
    public void MinigameUiDisable()
    {
        //_lastInteractable.EndInteractable();

        _bankUI.gameObject.SetActive(true);

        _miniGameUI.gameObject.SetActive(false);

        _shopUI.gameObject.SetActive(false);

        BtnHintUiDisable();

        MissingItemtUiDisable();
    }

    public void HudUiEnable()
    {
    }

    public void BtnHintUiEnable()
    {
        _showBtnHintUI.SetActive(true);
    }
    public void BtnHintUiDisable()
    {
        _showBtnHintUI.SetActive(false);
    }
    public void MissingItemtUiEnable()
    {
        _showMissingItemtUI.SetActive(true);
    }
    public void MissingItemtUiDisable()
    {
        _showMissingItemtUI.SetActive(false);
    }
}
