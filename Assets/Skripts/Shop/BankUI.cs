using TMPro;
using UnityEngine;

public class BankUI : MonoBehaviour
{
    [SerializeField] private Bank _bank;
    [SerializeField] private TMP_Text _textMoney;

    private void OnDisable()
    {
        _bank.MoneyChange -= ViewMoney;
    }

    private void OnEnable()
    {
        _bank.MoneyChange += ViewMoney;
        ViewMoney(_bank.Money);
    }

    private void ViewMoney(int currentMoney)
    {
        _textMoney.text = currentMoney.ToString();
    }
}
