using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weight : MonoBehaviour, IInteractable
{
    [SerializeField] private List<GameObject> _weights;

    private int _needCountWeight;

    private PlayerAll _player;
    private bool _isInteract = true;

    public void EndInteractable()
    {
        _player.CloseInteract();
    }

    public void Interact(PlayerAll player)
    {
        if (_needCountWeight > 0 && player.Inventory.ItemName == ItemName.Weight)
        {
            _needCountWeight--;
            _weights[_needCountWeight].SetActive(true);

            EndInteractable();

            if (_needCountWeight == 0)
            {
                transform.position = Vector3.zero;
            }
        }
        else
        {
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
