using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 10f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }
}