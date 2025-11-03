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

    public Item _visibleItem;
    private Item _activeSlot = null;

    private Keyboard _keyboard;
    private StoryItems _storyItems;

    public int CountPriceItem => _backpack.GetItem.Where(item => item.TypeItem == TypeItem.PriceItem).Count();

    public GameObject DropPoint => _dropPoint;
    public ItemName ItemName => _activeSlot != null ? _activeSlot.ItemName : ItemName.None;
    public Item VisibleItem => _visibleItem;

    public event UnityAction<Item> PickUpGarbage;
    public event UnityAction<Item> OnInteractWithStoryObject;

    private void Awake()
    {
        _keyboard = Keyboard.current;
        _storyItems = GetComponent<StoryItems>();
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

    public Item PopActiveSlot()
    {
        if (_activeSlot == null)
            return null;

        _activeSlot.transform.SetParent(null);
        _activeSlot.DropPoint();

        Item drop = _activeSlot;
        _activeSlot = null;

        return drop;
    }

    public void InventoryAction()
    {
        if (_visibleItem != null && _keyboard.eKey.wasPressedThisFrame)
        {
            Item currentItem = _visibleItem;  // ✅ Сохраняем ссылку

            // ✅ ВСЕ проверки через currentItem, а не _visibleItem
            if (currentItem != null && currentItem.TypeItem == TypeItem.GarbageItem)
            {
                PickUpGarbage?.Invoke(currentItem);
                currentItem.SetPoint(-1);
                Destroy(currentItem.gameObject);  // ✅ Явное уничтожение
                _visibleItem = null;
                return;
            }

            if (currentItem != null && currentItem.TypeItem == TypeItem.QuestItem)
            {
                OnInteractWithStoryObject?.Invoke(currentItem);
                currentItem.SetPoint(-1);
                _visibleItem = null;
                return;
            }

            if (currentItem != null && currentItem.TypeItem == TypeItem.PriceItem)
            {
                if (_backpack.AddItem(currentItem))
                    _visibleItem = null;
            }
            else if (currentItem != null && _activeSlot == null && currentItem.TypeItem == TypeItem.ActiveItem)
            {
                _activeSlot = currentItem;
                currentItem.transform.SetParent(transform);
                currentItem.SetPoint(-1);
                currentItem.transform.position = _activeSlotPoint.transform.position;
                _visibleItem = null;
            }
        }
        else if (_keyboard.qKey.wasPressedThisFrame)
        {
            if (_activeSlot != null)
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

    private void Update()
    {
        CleanupDestroyedItems();
        InventoryAction();
    }

    private void CleanupDestroyedItems()
    {
        if (_visibleItem != null && _visibleItem.gameObject == null)
            _visibleItem = null;

        if (_activeSlot != null && _activeSlot.gameObject == null)
            _activeSlot = null;
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
