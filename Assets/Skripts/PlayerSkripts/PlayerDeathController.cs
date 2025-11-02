using UnityEngine;
using UnityEngine.Events;
using System.Collections;


// Примечания по использованию:
// - Поместите Vignette Image на Canvas (Screen Space - Overlay), растяните на весь экран, цвет = черный, alpha = 0.
// - На игроке добавьте PlayerDeathController; назначьте Animator, AudioSource (опционально), VignetteFader (тот же объект, что на Canvas), и точку respawn.
// - Можно добавить компонент Respawner на игрока для корректной работы с Rigidbody.
// - В DeathTrigger (game object с Collider isTrigger) надо указать тег Player.
// - Используйте UnityEvents (OnDeathStart, OnBeforeRespawn, OnAfterRespawn, OnDeathComplete) чтобы добавить дополнительные эффекты в будущем (например UI, статистику, etc.).

[RequireComponent(typeof(Collider), typeof(PlayerEvents))]
public class PlayerDeathController : MonoBehaviour
{
    [Header("Animation & Sound")]
    public Animator animator; // Animator игрока
    public string deathTriggerName = "Die"; // trigger param name
    [Tooltip("Если не указан, используется animationDuration (см. ниже)")]
    public float animationDuration = 0.0f; // если > 0 — ждем столько секунд на анимацию
    public AudioSource audioSource; // звуковой источник на игроке
    public AudioClip deathClip;

    [Header("Vignette / Fade")]
    public VignetteFader vignetteFader; // компонент, который затемняет экран
    public float vignetteFadeDuration = 0.8f;
    public float vignetteHold = 0.4f; // время в затемнении перед респауном

    [Header("Respawn")]
    public Transform respawnPoint; // куда телепортировать
    public float postRespawnInvulnerability = 1.0f; // опционально

    [Header("Modules")]
    [Tooltip("Опционально: компоненты, реализующие IDisableable будут выключены на время смерти")]
    public MonoBehaviour[] disableableComponents;

    private PlayerEvents events;

    bool isDead = false;

    void Start()
    {
        events = GetComponent<PlayerEvents>();
    }

    public void Die(GameObject killer = null)
    {
        if (isDead) return;
        StartCoroutine(DeathSequence(killer));
    }

    IEnumerator DeathSequence(GameObject killer)
    {
        isDead = true;
        // 1) invoke start event
        events.OnDeathStart?.Invoke();

        // 2) отключаем управление
        foreach (var mb in disableableComponents)
        {
            if (mb == null) continue;
            if (mb is IDisableable dis)
                dis.DisableControl();
            else
                mb.enabled = false;
        }
        events.OnBeforeRespawn?.Invoke();

        // 3) play animation
        if (animator != null && !string.IsNullOrEmpty(deathTriggerName))
        {
            animator.SetTrigger(deathTriggerName);
        }

        // 4) play sound
        if (audioSource != null && deathClip != null)
        {
            audioSource.PlayOneShot(deathClip);
        }

        // 5) wait for animation duration OR a small default
        float waitAnim = 0.5f;
        if (animationDuration > 0f)
            waitAnim = animationDuration;
        else
        {
            // попытка вычислить длительность текущей анимации (если есть) — best-effort
            if (animator != null)
            {
                var state = animator.GetCurrentAnimatorStateInfo(0);
                // если анимация в стейте — берем её длину (может быть 0 если переход)
                if (state.length > 0.001f)
                    waitAnim = state.length;
            }
        }
        // безопасный cap
        waitAnim = Mathf.Max(waitAnim, 0.1f);
        yield return new WaitForSeconds(waitAnim);

        // 6) затемнение
        if (vignetteFader != null)
        {
            yield return StartCoroutine(vignetteFader.FadeTo(1f, vignetteFadeDuration));
        }
        else
        {
            yield return new WaitForSeconds(vignetteFadeDuration);
        }

        // небольшая пауза в затемнении
        yield return new WaitForSeconds(vignetteHold);

        // 7) Before respawn event
        events.OnBeforeRespawn?.Invoke();

        // 8) Корутина респауна (телепорт, сброс физики и т.д.)
        if (respawnPoint != null)
        {
            Respawner resp = GetComponent<Respawner>();
            if (resp != null)
            {
                resp.TeleportTo(respawnPoint);
            }
            else
            {
                // fallback: прямой телепорт и попытка обнулить Rigidbody
                transform.position = respawnPoint.position;
                transform.rotation = respawnPoint.rotation;
                var rb = GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
            }
        }

        events.OnAfterRespawn?.Invoke();

        // 9) возвращаем экран (fade out)
        if (vignetteFader != null)
        {
            yield return StartCoroutine(vignetteFader.FadeTo(0f, vignetteFadeDuration));
        }
        else
        {
            yield return new WaitForSeconds(vignetteFadeDuration);
        }

        // 10) даем время на инвулнериблити
        if (postRespawnInvulnerability > 0f)
            yield return new WaitForSeconds(postRespawnInvulnerability);

        // 11) включаем управление обратно
        foreach (var mb in disableableComponents)
        {
            if (mb == null) continue;
            if (mb is IDisableable dis)
                dis.EnableControl();
            else
                mb.enabled = true; // fallback — включить компонент
        }

        isDead = false;
        events.OnDeathComplete?.Invoke();
    }
}
