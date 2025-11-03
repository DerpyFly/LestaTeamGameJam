using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Torche : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _pointPositionPearl;

    private Torches _torches;

    private SphereCollider _sphereCollider;
    private PlayerAll _player;
    private bool _isInteract = true;

    private void Awake()
    {
        _torches = transform.GetComponentInParent<Torches>();
        _sphereCollider = transform.GetComponentInParent<SphereCollider>();
    }

    public void EndInteractable()
    {
        _player.UnlockInput();
        _player.CloseInteract();
    }

    public void Interact(PlayerAll playerAll)
    {
        _player.BlockInput();

        if (playerAll.Inventory.ItemName == ItemName.Pearl && _isInteract)
        {
            _sphereCollider.radius = 0;

            Item drop = _player.PopActiveSlot();

            if (drop == null)
                EndInteractable();

            drop.SetPoint(-1);
            drop.transform.position = _pointPositionPearl.transform.position;

            _torches.SetPearl();
        }

        EndInteractable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerAll player) && _isInteract)
        {
            if (_player == null)
                _player = player;

            if (other.gameObject.TryGetComponent(out PlayerEvents playerEvents) && player.Inventory.ItemName == ItemName.Pearl)
            {
                playerEvents.OnShowPressHint?.Invoke();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerEvents player))
        {
            player.OnInteractComplete?.Invoke();
        }
    }
}
