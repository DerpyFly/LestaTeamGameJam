using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Weight : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> _weights;

    private int _needCountWeight = 3;

    private PlayerAll _player;

    private SphereCollider _sphereCollider;

    public event UnityAction OnEndWeight;

    private void Awake()
    {
        _sphereCollider = GetComponent<SphereCollider>();
    }

    public void EndInteractable()
    {
        _player.UnlockInput();
        _player.CloseInteract();
    }

    public void Interact(PlayerAll player)
    {
        _player.BlockInput();

        if (_needCountWeight > 0 && player.Inventory.ItemName == ItemName.Weight)
        {
            Item drop = player.PopActiveSlot();

            if (drop == null)
                EndInteractable();

            drop.gameObject.SetActive(false);
            _needCountWeight--;
            _weights[_needCountWeight].SetActive(true);

            EndInteractable();

            if (_needCountWeight == 0)
            {
                _sphereCollider.radius = 0;
                OnEndWeight?.Invoke();
            }
        }
        else
        {
            if (_player != null)
                EndInteractable();  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerAll player) && _needCountWeight > 0)
        {
            if (_player == null)
                _player = player;

            if (player.Inventory.ItemName == ItemName.Weight)
                player.ViewWindowInteract(KeyCode.E);
            else
                player.ViewWindowInteract(KeyCode.E, ItemName.Weight);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerAll player) && _needCountWeight > 0)
        {
            player.ViewWindowInteract(KeyCode.None);
        }
    }
}
