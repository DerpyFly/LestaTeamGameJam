using System.Collections;
using UnityEngine;

public class RadiusItem : MonoBehaviour
{

    private Rigidbody _rb;
    private GameObject _target;

    private bool _isUpClose = true;
    private float _force;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (_target != null && !_isUpClose)
    //    {
    //        _isUpClose = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (_target != null && _isUpClose)
    //    {
    //        Debug.Log("Force");
    //        _isUpClose = false;
    //        StartCoroutine(ReturnPlayer());
    //    }
    //}

    public void Init(GameObject player, Rigidbody rb, float force)
    {
        _target = player;
        _rb = rb;
        _force = force;
    }

    //public void DropPlayer()
    //{
    //    StopAllCoroutines();
    //    _rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
    //    _rb.linearVelocity = Vector3.zero;
    //    _player = null;
    //}

    private IEnumerator ReturnPlayer()
    {
        while (!_isUpClose && _target != null)
        {
            _rb.AddForce((_target.transform.position - transform.position) * _force * Time.deltaTime);

            yield return null;
        }
    }
}
