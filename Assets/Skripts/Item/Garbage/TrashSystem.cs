using UnityEngine;
using UnityEngine.Events;

public class TrashSystem : MonoBehaviour
{
    [SerializeField] private Inventory _inventoryAction;

    public int CountTrash {  get; private set; }

    public event UnityAction<int> ChangeTrashCount;

    private void OnDisable()
    {
        _inventoryAction.PickUpGarbage -= AddTrash;
    }

    private void OnEnable()
    {
        _inventoryAction.PickUpGarbage += AddTrash;
    }

    private void Awake()
    {
        CountTrash = 0;
    }

    private void AddTrash(Item item)
    {
        item.gameObject.SetActive(false);
        CountTrash++;

        ChangeTrashCount?.Invoke(CountTrash);
    }
}
