using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Element : MonoBehaviour
{
    [SerializeField] private TMP_Text _textPrice;
    [SerializeField] private Button _button;

    private ShopUI _shopUI;
    private int _id;

    public int Id => _id;

    public void Init(ShopUI shopUI, int id, int price)
    {
        _shopUI = shopUI;
        _id = id;

        _textPrice.text = price.ToString();
    }

    public void Buy()
    {
        _shopUI.Buy(_id);
    }

    public void EnableButton()
    {
        _button.interactable = true;
    }

    public void DisableButton()
    {
        _button.interactable = false;
    }

    public void SetPrice(int price)
    {
        _textPrice.text = price.ToString();
    }
}
