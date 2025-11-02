using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boat : MonoBehaviour
{
    [SerializeField] private Weight _weight;
    [SerializeField] private GameObject _offPeople;
    [SerializeField] private GameObject _onPeople;
    [SerializeField] private float _timeWait;

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

        StartCoroutine(OnPeople());
    }

    private IEnumerator OnPeople()
    {
        yield return new WaitForSeconds(_timeWait);

        _offPeople.SetActive(false);
        _onPeople.SetActive(true);
    }
}
