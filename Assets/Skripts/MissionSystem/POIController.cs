using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;

[RequireComponent(typeof(Collider))]
public class POIController : MonoBehaviour
{
    [Header("Requirements")]
    [Tooltip("0 - ножницы, 1 - динамит, 2 - зажигалка")]
    [SerializeField] private List<bool> requiredItems; // 1 - ножницы, 2 - динамит, 3 - зажигалка

    [Header("Interaction")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private InputActionReference interactInput;

    [Header("Minigame")]
    [SerializeField] private MonoBehaviour minigameComponent; // компонент, реализующий IMinigame

    [Header("UI")]
    [SerializeField] private POIUIManager uiManager; // ссылка на UI менеджер (можно общий для сцены)

    private PlayerEvents events;
    bool playerInside = false;
    bool pressedInteract = false;
    GameObject playerObj;
    List<(int, string)> missing = new List<(int, string)>();

    void Awake()
    {
        interactInput.action.performed += OnInteracPressed;
    }

    void OnDestroy()
    {
        interactInput.action.performed -= OnInteracPressed;
    }

    void Reset()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInside = true;
        playerObj = other.gameObject;
        events = playerObj.GetComponent<PlayerEvents>();

        // Проверяем инвентарь
        var storyItemsComp = playerObj.GetComponent<StoryItems>();

        if (storyItemsComp == null)
            return;


        for (int i = 0; i < storyItemsComp.storyItems.Count; i++)
        {
            if (!storyItemsComp.storyItems[i] && requiredItems[i] != storyItemsComp.storyItems[i])
            {
                switch(i)
                {
                    case 0:
                        missing.Add((i, "ножницы"));
                        break;
                    case 1:
                        missing.Add((i, "динамит"));
                        break;
                    case 2:
                        missing.Add((i, "зажигалка"));
                        break;
                }
                
            }
        }

        // Показываем UI: либо Press E, либо список недостающих предметов
        if (missing.Count == 0)
        {
            events.OnShowPressHint?.Invoke();
            // uiManager?.ShowPressButton();
        }
        else
        {
            uiManager?.SetMissingItems(missing);
            events.OnInteractNotEnoughItems?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        playerInside = false;
        playerObj = null;
        missing.Clear();
        uiManager?.HideAll();
    }

    private void OnInteracPressed(InputAction.CallbackContext ctx)
    {
        if (!playerInside || playerObj == null) return;

        if (missing.Count == 0)
        {
            Debug.Log("Pressed E");
            StartInteraction();
        }
    }

    void Update()
    {

    }

    void StartInteraction()
    {
        events.OnInteractStart?.Invoke();
        // uiManager?.HideAll();

        // Запускаем мини-игру если есть
        if (minigameComponent != null && minigameComponent is IMinigame)
        {
            var mg = minigameComponent as IMinigame;
            mg.StartMinigame(playerObj, (success) =>
            {
                // по завершении — вызываем событие завершения
                events.OnInteractComplete?.Invoke();
            });
        }
        else
        {
            // Если мини-игры нет — просто вызываем OnInteractComplete
            events.OnInteractComplete?.Invoke();
        }

        
    }
}