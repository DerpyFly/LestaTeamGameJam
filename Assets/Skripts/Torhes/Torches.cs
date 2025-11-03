using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Torches : MonoBehaviour
{
    [SerializeField] private List<Torche> _torches;

    private int _torcheCount;

    public event UnityAction OnFinalPearl;

    private void Awake()
    {
        _torcheCount = _torches.Count;
    }

    public void SetPearl()
    {
        _torcheCount--;

        if (_torcheCount == 0)
        {
            OnFinalPearl?.Invoke();
            Debug.Log("Final!!!");
        }
    }
}
