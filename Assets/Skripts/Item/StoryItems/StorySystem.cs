using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class StorySystem : MonoBehaviour
{
    [SerializeField] private Inventory _inventoryAction;
    [SerializeField] private StoryItems storyItems;
    [SerializeField] private PlayerAll playerAll;
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private GameObject brokenPropellerPrefab;
    [SerializeField] private ScrewRotator screwRotator1;
    [SerializeField] private ScrewRotator screwRotator2;
    [SerializeField] private Transform propellerTransformForNet;
    [SerializeField] private WaypointMovement boatWaypointMovement;
    [SerializeField] private GameObject dynamiteGhostPrefab;
    [SerializeField] private Transform dynamiteSpawnTransform;
    [SerializeField] private GameObject dynamitePrefab;
    [SerializeField] private GameObject bossBoat;

    private GameObject spawnedGhostDynamite;
    private GameObject spawnedDynamite;

    private void Start()
    {
        if (_inventoryAction == null)
            _inventoryAction = GetComponent<Inventory>();
        if (storyItems == null)
            storyItems = GetComponent<StoryItems>();
        if (playerAll == null)
            playerAll = GetComponent<PlayerAll>();
    }
    private void OnEnable()
    {
        _inventoryAction.OnInteractWithStoryObject += CheckStoryObject;
    }

    private void OnDisable()
    {
        _inventoryAction.OnInteractWithStoryObject -= CheckStoryObject;
    }
    
    private void CheckStoryObject (Item obj)
    {
        switch(obj.ItemName)
        {
            case ItemName.Net:
                if (storyItems.storyItems[0])
                {
                    Instantiate(netPrefab, obj.transform.position, Quaternion.identity);
                }
                break;
            case ItemName.Propeller:
                if (playerAll.Inventory.ItemName == ItemName.Net)
                {
                    screwRotator1.StopRotation();
                    screwRotator2.StopRotation();
                    boatWaypointMovement.enabled = false;
                    Instantiate(brokenPropellerPrefab, propellerTransformForNet);
                    spawnedGhostDynamite = Instantiate(dynamiteGhostPrefab, dynamiteSpawnTransform);
                }
                break;
            case ItemName.DynamitePlace:
                if (storyItems.storyItems[1])
                {
                    spawnedDynamite = Instantiate(dynamitePrefab, dynamiteSpawnTransform);
                    Destroy(spawnedGhostDynamite);
                }
                break;
            case ItemName.Dynamite:
                if (storyItems.storyItems[2])
                {
                    // TODO: call dynamite explosion
                    Destroy(spawnedDynamite);
                    // TODO: make hole in boat
                    bossBoat.GetComponent<Rigidbody>().useGravity = true;
                }
                break;
            default:
                break;
        }
        
    }
}