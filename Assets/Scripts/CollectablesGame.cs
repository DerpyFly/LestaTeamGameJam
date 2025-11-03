using UnityEngine;

public class CollectablesGame : MonoBehaviour
{
    public int _collectableCount;
   
    [SerializeField] private GameObject spawnObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void addItem()
    {
        _collectableCount++;

        if (_collectableCount >= 7)
        {
           spawnObject.SetActive(true);
        }
    }
}
