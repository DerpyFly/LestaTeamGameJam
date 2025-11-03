using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Backpack : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _minSpeed = 5;
    [SerializeField] private float _force = 500;
    [SerializeField] private Vector3 _deltaYXZ;
    [SerializeField] private float _returnForce = 500;
    [SerializeField] private float _radius = 3; 
    [SerializeField] private List<GameObject> _points = new();

    private Inventory _player;
    private Stack<Item> _items = new();
    private List<List<int>> _pointValue = new();

    private Rigidbody _rb;

    public List<Item> GetItem => _items.ToList();

    private void FixedUpdate()
    {
        Vector3 target = _player.transform.position + _player.transform.TransformDirection(_deltaYXZ);

        Vector3 direction = (transform.position - target).normalized;

        float currentDistance = Vector3.Distance(target, transform.position);

        if (currentDistance > _radius)
        {
            Vector3 desiredPosition = target + direction * _radius;
            Vector3 force = desiredPosition - transform.position;

            if (force.magnitude > _radius)
                _rb.AddForce(force.normalized * _force * Time.deltaTime);
        }
        else
        {

            if (_rb.linearVelocity.magnitude > _minSpeed)
                _rb.AddForce(-_rb.linearVelocity.normalized * _returnForce * Time.deltaTime);
        }
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
    }

    public void Init(Inventory player)
    {
        _rb = GetComponent<Rigidbody>();

        _player = player;
        
        foreach (GameObject point in _points)
            _pointValue.Add(new() { 0 });
    }

    public bool AddItem(Item newItem)
    {
        int emptySlot = SearchEmptySlot();

        if (emptySlot == -1)
            return false;

        _items.Push(newItem);
        newItem.SetPoint(emptySlot);
        newItem.transform.SetParent(transform);
        newItem.transform.position = _points[emptySlot].transform.position;
        newItem.transform.localScale = _points[emptySlot].transform.localScale;

        return true;
    }

    public Item DropItem()
    {
        if (_items.Count == 0)
            return null;

        Item drop = _items.Peek();
        _items.Pop();

        int dropPoint = drop.DropPoint();
        drop.transform.position = _player.DropPoint.transform.position;
        _pointValue[dropPoint][0] = 0;

        return drop;
    }

    public List<Item> DropPriceItem()
    {
        List<Item> clearStack = new();
        List<Item> dropItem = new();

        while(_items.Count > 0)
        {
            Item item = _items.Pop();

            if (item.TypeItem == TypeItem.PriceItem)
            {
                dropItem.Add(item);
                int dropPoint = item.DropPoint();
                _pointValue[dropPoint][0] = 0;
            }
            else
            {
                clearStack.Add(item);
            }
        }

        clearStack.Reverse();

        foreach (Item item in clearStack)
            _items.Push(item);

        return dropItem;
    }

    private int SearchEmptySlot()
    {
        for (int i = 0; i < _pointValue.Count; i++)
        {
            if (_pointValue[i][0] == 0)
            {
                _pointValue[i][0] = 1;

                return i;
            }
        }

        return -1;
    }
}
