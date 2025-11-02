using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DeathTrigger : MonoBehaviour
{
    [Tooltip("Какие объекты считать игроком")]
    public string targetTag = "Player"; // Ищем игрока по Тэгу Player

    private void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!string.IsNullOrEmpty(targetTag))
        {
            if (!other.CompareTag(targetTag)) return;
        }

        // fallback: если нет интерфейса — попробуем найти компонент PlayerDeathController
        var pdc = other.GetComponent<PlayerDeathController>();
        if (pdc != null)
        {
            pdc.Die(gameObject);
        }
    }
}