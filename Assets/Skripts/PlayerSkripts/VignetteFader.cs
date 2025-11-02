using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VignetteFader : MonoBehaviour
{
    public Image vignetteImage; // full-screen Image, Alpha управляет затемнением

    private void Reset()
    {
        // попытка найти Image на себе
        if (vignetteImage == null)
            vignetteImage = GetComponent<Image>();
    }

    void Awake()
    {
        if (vignetteImage == null)
            Debug.LogWarning("VignetteFader: Image не назначен.");
    }

    // Плавный переход альфы к targetAlpha за duration секунд
    public IEnumerator FadeTo(float targetAlpha, float duration)
    {
        if (vignetteImage == null)
            yield break;


        float start = vignetteImage.color.a;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, targetAlpha, duration > 0f ? (t / duration) : 1f);
            Color c = vignetteImage.color;
            c.a = a;
            vignetteImage.color = c;
            yield return null;
        }
        Color end = vignetteImage.color;
        end.a = targetAlpha;
        vignetteImage.color = end;
    }
}