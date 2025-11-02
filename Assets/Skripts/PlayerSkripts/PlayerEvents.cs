using UnityEngine;
using UnityEngine.Events;

public class PlayerEvents : MonoBehaviour
{
    [Header("POI Events")]
    public UnityEvent OnInteractNotEnoughItems; // вызывается если не хватает вещей
    public UnityEvent OnShowPressHint; // вызывается если игрок зашел в триггер и у него есть все вредметы
    public UnityEvent OnInteractStart; // при старте взаимодействия (после проверки, что есть предметы)
    public UnityEvent OnInteractComplete; // при завершении (всегда)

    [Header("Death Events")]
    public UnityEvent OnDeathStart; // вызывается сразу при старте смерт. последовательности
    public UnityEvent OnBeforeRespawn; // до телепорта
    public UnityEvent OnAfterRespawn; // сразу после телепорта
    public UnityEvent OnDeathComplete; // когда вся последовательность завершена

    //действие для отображения окошка "нажмите E"
    public event UnityAction OnActionE;
    //действие для отображения окошка "нажмите Q"
    public event UnityAction OnActionQ;
    //действие при выходе из триггера взаимодействия
    public event UnityAction OnCloseWindow;

    public void EnableWindowInteract(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.E:
                OnActionE?.Invoke();
                break;
            case KeyCode.Q:
                OnActionQ?.Invoke();
                break;
        }
    }

    public void EnableWindowNotFoundItem(ItemName notFoundItem)
    {
        
    }

    public void DisableWindowInteract()
    {
        OnCloseWindow?.Invoke();
    }
}