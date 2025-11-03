using UnityEngine;

public class GetCollectableStar : MonoBehaviour
{
    private CollectablesGame game;

    private void OnTriggerEnter(Collider other)
    {

        //if (other.CompareTag("Player"))
        //{
        //    CollectablesGame game = other.GetComponent<CollectablesGame>();
        //    game.addItem();
        //    Destroy(gameObject);
        //}
    }
}
