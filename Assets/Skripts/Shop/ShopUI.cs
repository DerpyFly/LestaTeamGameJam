using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Bank _bank;
    [Space]
    [SerializeField] private TMP_Text _textCountItem;
    [SerializeField] private TMP_Text _textMoney;
    [SerializeField] private string _textNameMoney = "Coins";
    [SerializeField] private GameObject _content;
    [Space]
    [SerializeField] private List<ShopElement> _shopElementsPrefab = new();

    private PlayerAll _player;
    private Shop _shop;

    private List<ShopElement> _shopElements = new();
    
    private void Awake()
    {
        for (int i = 0; i < _shopElementsPrefab.Count; i++)
        {
            Element newElement = Instantiate(_shopElementsPrefab[i].Element, _content.transform);
            newElement.Init(this, i, _shopElementsPrefab[i].Price, _shopElementsPrefab[i].Icon);
            ShopElement newShopElements = new()
            {
                Element = newElement,
                PrefabItem = _shopElementsPrefab[i].PrefabItem,
                Price = _shopElementsPrefab[i].Price
            };

            _shopElements.Add(newShopElements);

            if (i >= 1)
                newElement.DisableButton();
        }
    }

    public void ViewInventory(PlayerAll player = null, IInteractable interactable = null)
    {
        if (player != null)
            _player = player;

        if(interactable != null && interactable is Shop)
            _shop = interactable as Shop;

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

    public void Buy(int id)
    {
        ShopElement shopElement = _shopElements[id];

        bool resultBuy = _bank.TryPay(shopElement.Price); //RESULT BUY!!!!

        if(resultBuy)
        {
            _shop.SpawnItem(shopElement.PrefabItem);
            shopElement.Element.DisableButton();

            if(id + 1 < _shopElements.Count)
            {
                _shopElements[id + 1].Element.EnableButton();
            }

            ViewInventory();
        }
    }
}

[Serializable]
public class ShopElement
{
    public Element Element;
    public Item PrefabItem;
    public Sprite Icon;
    public int Price;
}