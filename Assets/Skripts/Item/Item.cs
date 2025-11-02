using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class Item : MonoBehaviour
{
    [SerializeField] private TypeItem _typeItem;

    private bool _isFree = true;
    private int _id;

    private Rigidbody _rb;
    private BoxCollider _boxCollider;

    public TypeItem TypeItem => _typeItem;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
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
        _boxCollider.enabled = false;
        _rb.isKinematic = true;
        _rb.constraints = RigidbodyConstraints.FreezePositionY;
        _id = id;
    }

    public int DropPoint()
    {
        _isFree = true;
        _rb.isKinematic = false;
        _boxCollider.enabled = true;
        _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
        return _id;
    }
}
