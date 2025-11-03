using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Torche : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _pointPositionPearl;

    private Torches _torches;

    private SphereCollider _sphereCollider;

    private void Awake()
    {
        _torches = transform.GetComponentInParent<Torches>();
    }

    public void EndInteractable()
    {
        throw new System.NotImplementedException();
    }

    public void Interact(PlayerAll playerAll)
    {
        
    }
}
