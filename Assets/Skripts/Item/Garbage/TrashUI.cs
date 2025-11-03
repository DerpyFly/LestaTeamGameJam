using TMPro;
using UnityEngine;

public class TrashUI : MonoBehaviour
{
    [SerializeField] private TrashSystem _trashSystem;
    [SerializeField] private TMP_Text _textCountTrash;
    [SerializeField] private string _textNameLine = "Собранно мусора: ";
    [SerializeField] private WindowManager _windowManager;

    private void OnEnable()
    {
        if( _trashSystem != null )
            _trashSystem.ChangeTrashCount += OnChangeTrashCount;
    }

    private void OnDisable()
    {
        if (_trashSystem != null)
            _trashSystem.ChangeTrashCount -= OnChangeTrashCount;
    }

    private void Awake()
    {
        if (_trashSystem == null)
            Debug.LogError("TrashSystem is NULL!");
        
        if (_textCountTrash == null)
            Debug.LogError("Text is NULL!");
    }

    private void OnChangeTrashCount(int newCount)
    {
        if (_textCountTrash.enabled == false)
            _textCountTrash.enabled = true;

        _textCountTrash.text = _textNameLine + newCount.ToString() + " из 7";

        if(newCount >= 7)
        {
            _windowManager.StartNotification();
        }
    }
}
