using UnityEngine;

public class TrashSystem : MonoBehaviour
{
    [SerializeField] private Inventory _inventoryAction;

    public int CountTrash {  get; private set; }

    private void OnDisable()
    {
        _inventoryAction.PickUpGarbage -= AddTrash;
    }

    private void Awake()
    {
        _inventoryAction.PickUpGarbage += AddTrash;
        CountTrash = 0;
    }

    private void AddTrash(Item item)
    {
        item.gameObject.SetActive(false);
        CountTrash++;
    }
}
