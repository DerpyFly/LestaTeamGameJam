using TMPro;
using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _textCountItem;
    [SerializeField] private TMP_Text _textMoney;
    [SerializeField] private string _textNameMoney = "Coins";

    private PlayerAll _player;

    public void ViewInventory(PlayerAll player = null)
    {
        if (player != null)
            _player = player;

        int countPriceItem = _player.Inventory.CountPriceItem;
        int money = _player.Bank.Money;

        _textCountItem.text = countPriceItem.ToString();
        _textMoney.text = money.ToString() + " " + _textNameMoney;
    }

    public void SellAll()
    {
        int newMoney = _player.Inventory.SellItem();
        _player.Bank.AddMoney(newMoney);
        ViewInventory();
    }
}
