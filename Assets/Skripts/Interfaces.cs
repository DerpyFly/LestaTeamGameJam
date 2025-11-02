using UnityEngine;
using System.Collections.Generic;
public interface IDisableable
{
    void DisableControl();
    void EnableControl();
}

public interface IMinigame
{
    void StartMinigame(GameObject player, System.Action<bool> onComplete);
}