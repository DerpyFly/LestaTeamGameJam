using Unity.VisualScripting;
using UnityEngine;

public class MiniGameWithGiryas : MonoBehaviour
{
    public int GiryaCounter;
    public GameObject Girya;

    private void AddGirya()
    {
        GiryaCounter++;
    }
    private void CheckGiryas()
    {
        if (GiryaCounter >= 3)
        {

        }
    }
    private void FinishGame()
    {
        
    }
}