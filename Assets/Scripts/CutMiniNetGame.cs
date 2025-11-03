using UnityEngine;
using System.Collections;

public class CutMiniNetGame : MonoBehaviour
{
    [SerializeField] private GameObject closedNet;
    [SerializeField] private GameObject openedNet;
    [SerializeField] private GameObject casedFish;
    [SerializeField] private WaypointMovement waypointMovement;
    [SerializeField] private Transform farWaypointTransform;
    [SerializeField] private float fishDisappearDelay = 3f;

    private bool playerInRange = false;

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void NetCutted()
    {
        closedNet.SetActive(false);
        openedNet.SetActive(true);
        ChangeWaypoint();



    }

    private void OnTriggerExit(Collider other)
    {
        // Проверяем, что вышел игрок
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        // suda vstavit najatie E BUTTON
        if (playerInRange)
        {
            NetCutted();
        }
    }

    private void ChangeWaypoint()
    {
        waypointMovement.AddWaypoint(farWaypointTransform);
        StartCoroutine(DisappearFishAfterDelay());
    }

    private IEnumerator DisappearFishAfterDelay()
    {
        // Ждем указанное количество секунд
        yield return new WaitForSeconds(fishDisappearDelay);

         //Mojno v teorii Spawn SellItem
        if (casedFish != null)
        {
            casedFish.SetActive(false);
        }
    }



}
