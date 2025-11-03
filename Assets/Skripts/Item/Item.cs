using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Item : MonoBehaviour
{
    [SerializeField] private TypeItem _typeItem;
    [SerializeField] private ItemName _itemName;
    [SerializeField] private int _price = 10;

    private bool _isFree = true;
    private int _id;

    private Rigidbody _rb;
    private Collider _collider;

    public TypeItem TypeItem => _typeItem;
    public ItemName ItemName => _itemName;
    public int Price => _price;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        var cols = GetComponents<Collider>();
        foreach (var col in cols)
        {
            if (col.isTrigger == false)
            {
                _collider = col;
                break;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isFree && other.gameObject.TryGetComponent(out Inventory player))
        {
            player.SetActiveItem(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isFree && other.gameObject.TryGetComponent(out Inventory player))
        {
            player.DeleteActiveItem(this);
        }
    }

    public void SetPoint(int id)
    {
        _isFree = false;
        _collider.enabled = false;
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints.FreezePositionY;
        _id = id;
    }

    public int DropPoint()
    {
        _isFree = true;
        transform.SetParent(null);
        _rb.isKinematic = false;
        _collider.enabled = true;
        _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        return _id;
    }
}
