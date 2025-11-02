using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Backpack : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _minSpeed = 5;
    [SerializeField] private float _force = 500;
    [SerializeField] private float _deltaY = 0.3f;
    [SerializeField] private float _returnForce = 500;
    [SerializeField] private float _length = 10;
    [SerializeField] private List<GameObject> _points = new();

    private Inventory _player;
    private Stack<Item> _items = new();
    private List<List<int>> _pointValue = new();

    private Rigidbody _rb;

    private void Update()
    {
        transform.LookAt(new Vector3(_player.transform.position.x, transform.position.y, _player.transform.position.z));
    }

    private void FixedUpdate()
    {
        Vector3 target = new Vector3(_player.transform.position.x, _player.transform.position.y + _deltaY, _player.transform.position.z);

        if ((target - transform.position).magnitude > _length && _rb.linearVelocity.magnitude < _maxSpeed)
            _rb.AddForce((target - transform.position) * _force * Time.deltaTime);
        else if ((target - transform.position).magnitude < _length && _rb.linearVelocity.magnitude > _minSpeed)
            _rb.AddForce(-_rb.linearVelocity.normalized * _returnForce * Time.deltaTime);
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

        return true;
    }

    public Item DropItem()
    {
        if (_items.Count == 0)
            return null;

        Item drop = _items.Peek();
        _items.Pop();

        int dropPoint = _items.Peek().DropPoint();
        drop.transform.SetParent(null);
        drop.transform.position = _player.DropPoint.transform.position;
        _pointValue[dropPoint][0] = 0;

        return drop;
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
