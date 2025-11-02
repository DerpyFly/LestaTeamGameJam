using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject _dropPoint;
    [SerializeField] private GameObject _activeSlotPoint;
    [SerializeField] private Backpack _backpack;

    private Item _visibleItem;
    private Item _activeSlot = null;

    private Keyboard _keyboard;

    public int CountPriceItem => _backpack.GetItem.Where(item => item.TypeItem == TypeItem.PriceItem).Count();

    public GameObject DropPoint => _dropPoint;
    public ItemName ItemName => _activeSlot.ItemName;

    public event UnityAction<Item> PickUpGarbage;

    private void Awake()
    {
        _keyboard = Keyboard.current;
        _backpack.Init(this);
    }

    public void SetActiveItem(Item newItem)
    {
        if (_visibleItem != null)
            if((transform.position - newItem.transform.position).magnitude > (transform.position - _visibleItem.transform.position).magnitude)
                return;

        _visibleItem = newItem;
    }

    public void DeleteActiveItem(Item newItem)
    {
        if (newItem != _visibleItem)
            return;

        _visibleItem = null;
    }

    public void InventoryAction()
    {
        if (_visibleItem != null && _keyboard.eKey.wasPressedThisFrame)
        {
            if(_visibleItem.TypeItem == TypeItem.GarbageItem)
            {
                PickUpGarbage?.Invoke(_visibleItem);
                _visibleItem.SetPoint(-1);
                _visibleItem = null;

                return;
            }

            if(_visibleItem.TypeItem == TypeItem.PriceItem || (_visibleItem.TypeItem == TypeItem.ActiveItem && _activeSlot != null))
            {
                if (_backpack.AddItem(_visibleItem))
                    _visibleItem = null;
            }
            else if(_activeSlot == null && _visibleItem.TypeItem == TypeItem.ActiveItem)
            {
                _activeSlot = _visibleItem;
                _visibleItem.transform.SetParent(transform);
                _visibleItem.SetPoint(-1);
                _visibleItem.transform.position = _activeSlotPoint.transform.position;
                _visibleItem = null;
            }
        }
        else if (_keyboard.qKey.wasPressedThisFrame)
        {
            if(_activeSlot != null)
            {
                _activeSlot.transform.SetParent(null);
                _activeSlot.DropPoint();
                _activeSlot.transform.position = _dropPoint.transform.position;
                _activeSlot = null;

                return;
            }


            _backpack.DropItem();
        }
    }

    public int SellItem()
    {
        int allSum = 0;

        List<Item> cellItems = _backpack.DropPriceItem();

        foreach(Item item in cellItems) {
            allSum += item.Price;
            item.gameObject.SetActive(false);
        }

        return allSum;
    }
}
