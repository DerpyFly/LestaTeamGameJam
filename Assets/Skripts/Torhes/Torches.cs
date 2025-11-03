using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Torches : MonoBehaviour
{
    [SerializeField] private List<Torche> _torches;

    private int _torcheCount;

    public event UnityAction OnFinalPearl;
    [SerializeField] private WaypointMovement waypointMovement;

    private void Awake()
    {
        _torcheCount = _torches.Count;
    }

    public void SetPearl()
    {
        _torcheCount--;

        if (_torcheCount == 0)
        {
            waypointMovement.enabled = true;
        }
    }
}
