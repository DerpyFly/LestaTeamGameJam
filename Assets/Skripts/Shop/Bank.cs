using UnityEngine;
using UnityEngine.Events;

public class Bank : MonoBehaviour
{
    [SerializeField] private int _startMoney = 0;

    private int _money;

    public int Money => _money;

    public event UnityAction<int> MoneyChange;

    private void Awake()
    {
        _money = _startMoney;
        MoneyChange?.Invoke(_money);
    }

    public void AddMoney(int money)
    {
        _money += money;
        MoneyChange?.Invoke(_money);
    }

    public bool TryPay(int price)
    {
        if(price >  _money)
            return false;

        _money -= price;

        MoneyChange?.Invoke(_money);

        return true;
    }
}
