using UnityEngine;

public class PlayerAll : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    private void Update()
    {
        _inventory.InventoryAction();
    }
}
