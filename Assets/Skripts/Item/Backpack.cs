using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Backpack : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 5;
    [SerializeField] private float _minSpeed = 5;
    [SerializeField] private float _force = 500;
    [SerializeField] private float _returnForce = 500;
    [SerializeField] private float _length = 10;
    [SerializeField] private List<GameObject> _points = new();

    private PlayerTest _player;
    private Stack<Item> _items = new();
    private List<List<int>> _pointValue = new();

    private Keyboard _keyboard;
    private Rigidbody _rb;

    private void Update()
    {
        transform.LookAt(_player.transform);
    }

    private void FixedUpdate()
    {
        if ((_player.transform.position - transform.position).magnitude > _length && _rb.linearVelocity.magnitude < _maxSpeed)
            _rb.AddForce((_player.transform.position - transform.position) * _force * Time.deltaTime);
        else if((_player.transform.position - transform.position).magnitude < _length && _rb.linearVelocity.magnitude > _minSpeed)
            _rb.AddForce(-_rb.linearVelocity.normalized * _returnForce * Time.deltaTime);
    }

    public void Init(PlayerTest player)
    {
        _keyboard = Keyboard.current;
        _rb = GetComponent<Rigidbody>();

        _player = player;

        foreach(GameObject point in _points)
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

    public void DropItem()
    {
        if (_items.Count == 0)
            return;

        int dropPoint = _items.Peek().DropPoint();
        _items.Peek().transform.SetParent(null);
        _items.Peek().transform.position = _player.DropPoint.transform.position;
        _pointValue[dropPoint][0] = 0;
        _items.Pop();
    }

    private int SearchEmptySlot()
    {
        for(int i = 0; i < _pointValue.Count; i++)
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
