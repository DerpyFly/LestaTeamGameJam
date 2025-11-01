using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Progress;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private GameObject _dropPoint;
    [SerializeField] private GameObject _activeSlotPoint;
    [SerializeField] private Backpack _backpack;

    private Item _visibleItem;
    private Item _activeSlot = null;

    private Keyboard _keyboard;

    public GameObject DropPoint => _dropPoint;

    private void Awake()
    {
        _keyboard = Keyboard.current;
        _backpack.Init(this);
    }

    public void SetActiveItem(Item newItem)
    {
        if (_visibleItem != null)
            return;

        _visibleItem = newItem;
    }

    public void DeleteActiveItem(Item newItem)
    {
        if (newItem != _visibleItem)
            return;

        _visibleItem = null;
    }

    private void Update()
    {
        InventoryAction();
    }

    private void InventoryAction()
    {
        if (_visibleItem != null && _keyboard.eKey.wasPressedThisFrame)
        {
            if(_visibleItem.TypeItem == TypeItem.PriceItem)
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
}
