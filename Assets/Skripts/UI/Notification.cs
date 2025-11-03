using System.Collections;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private string _textLine;
    [SerializeField] private float _timeLife;
    [SerializeField] private float _deltaPos;

    public void StartAnim()
    {
        StartCoroutine(Anim());
    }

    private IEnumerator Anim()
    {
        gameObject.SetActive(false);

        while (_timeLife >= 0)
        {
            transform.position += Vector3.up * _deltaPos;
            yield return null;

            _timeLife -= Time.deltaTime;
        }

        gameObject.SetActive(false);
    }
}
