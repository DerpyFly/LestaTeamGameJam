using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class POIUIManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject pressBtnPanel; // панель с изображением "Press E"
    [SerializeField] private TMP_Text pressBtnText; // текст (например, "Press E")


    [SerializeField] private GameObject missingPanel; // панель с сообщением о недостающих предметах
    [SerializeField] private TMP_Text missingText; // здесь перечисляются недостающие предметы
    [SerializeField] private PlayerEvents events; // Player events class
    private List<(int, string)> missingItems = new List<(int, string)>();

    void Start()
    {
        HideAll();
        if (events == null)
        {
            Debug.Log("NO PLAYER EVENTS IN PROPERTY WAS ASSIGHNED");
            Destroy(gameObject);
        }

        events.OnShowPressHint.AddListener(ShowPressButton);
        events.OnInteractNotEnoughItems.AddListener(() => ShowMissingItems(missingItems));
        events.OnInteractStart.AddListener(HideAll);
    }

    void Reset()
    {
        if (pressBtnPanel == null)
        {
            var p = transform.Find("PressBtnPanel");
            if (p != null) pressBtnPanel = p.gameObject;
        }
        if (missingPanel == null)
        {
            var m = transform.Find("MissingPanel");
            if (m != null) missingPanel = m.gameObject;
        }
    }

    public void ShowPressButton()
    {
        if (pressBtnPanel != null) pressBtnPanel.SetActive(true);
        if (missingPanel != null) missingPanel.SetActive(false);
    }

    public void ShowMissingItems(List<(int, string)> missing)
    {
        if (pressBtnPanel != null) pressBtnPanel.SetActive(false);
        if (missingPanel != null) missingPanel.SetActive(true);
        if (missingText != null)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Нет предметов:");
            foreach ((int _, string itemName) in missing)
            {
                // 1 - ножницы, 2 - динамит, 3 - зажигалка
                sb.AppendLine($"- {itemName}");

                
            }
            missingText.text = sb.ToString();
        }
    }

    public void HideAll()
    {
        if (pressBtnPanel != null) pressBtnPanel.SetActive(false);
        if (missingPanel != null) missingPanel.SetActive(false);
    }
    
    public void SetMissingItems(List<(int, string)> newList)
    {
        missingItems.Clear();
        foreach (var item in newList)
        {
            missingItems.Add(item);
        }
    }
}