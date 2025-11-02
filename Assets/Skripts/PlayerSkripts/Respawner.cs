using UnityEngine;

public class Respawner : MonoBehaviour
{
    [Tooltip("Если указан, будет сбрасывать linearVelocity/angVelocity у Rigidbody")]
    public void TeleportTo(Transform spawnPoint)
    {
        if (spawnPoint == null) return;
        var rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.position = spawnPoint.position;
            rb.rotation = spawnPoint.rotation;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        else
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }
    }
}