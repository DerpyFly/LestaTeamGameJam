using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boat : MonoBehaviour
{
    [SerializeField] private Weight _weight;

    private Rigidbody _rb;

    private void OnDisable()
    {
        _weight.OnEndWeight -= Drown;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _weight.OnEndWeight += Drown;
    }

    public void Drown()
    {
        _rb.isKinematic = false;
    }
}
