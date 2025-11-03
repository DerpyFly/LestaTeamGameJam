using UnityEngine;

[RequireComponent(typeof(Inventory))]
public class StorySystem : MonoBehaviour
{
    [SerializeField] private Inventory _inventoryAction;
    [SerializeField] private StoryItems storyItems;
    [SerializeField] private PlayerAll playerAll;
    [SerializeField] private GameObject netPrefab;
    [SerializeField] private GameObject netOnPropeller;
    [SerializeField] private ScrewRotator screwRotator1;
    [SerializeField] private ScrewRotator screwRotator2;
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
            case ItemName.Scissors:
                storyItems.storyItems[0] = true;
                Destroy(obj.gameObject);
                break;
            case ItemName.Dynamite:
                storyItems.storyItems[1] = true;
                Destroy(obj.gameObject);
                break;
            case ItemName.Lighter:
                storyItems.storyItems[2] = true;
                Destroy(obj.gameObject);
                break;
            case ItemName.UseScissors:
                if (storyItems.storyItems[0])
                {
                    Instantiate(netPrefab, obj.transform.position, Quaternion.identity);
                    Destroy(obj.gameObject);
                }
                break;
            case ItemName.BreakPropeller:
                if (playerAll.Inventory.ItemName == ItemName.Net)
                {
                    screwRotator1.StopRotation();
                    screwRotator2.StopRotation();
                    boatWaypointMovement.enabled = false;
                    netOnPropeller.SetActive(true);

                    spawnedGhostDynamite = Instantiate(dynamiteGhostPrefab, dynamiteSpawnTransform);
                    spawnedGhostDynamite.transform.localPosition = new Vector3(0f, 0f, 0.19f);
                    spawnedGhostDynamite.transform.Rotate(new Vector3(0, 90, 0));
                    Destroy(playerAll.Inventory._activeSlot.gameObject);
                    playerAll.Inventory._activeSlot = null;                    
                }
                break;
            case ItemName.PlaceDynamite:
                if (storyItems.storyItems[1])
                {
                    spawnedDynamite = Instantiate(dynamitePrefab, dynamiteSpawnTransform);
                    spawnedDynamite.transform.localPosition = new Vector3(0f, 0f, 0.19f);
                    spawnedDynamite.transform.Rotate(new Vector3(0, 90, 0));
                    Destroy(spawnedGhostDynamite);
                }
                break;
            case ItemName.Boom:
                if (storyItems.storyItems[2])
                {
                    // TODO: call dynamite explosion
                    Destroy(spawnedDynamite);
                    // TODO: make hole in boat
                    Rigidbody boatRB = bossBoat.GetComponent<Rigidbody>();
                    boatRB.useGravity = true;
                    boatRB.freezeRotation = false;
                    boatRB.constraints = RigidbodyConstraints.None;
                }
                else
                {
                    
                }
                break;
            default:
                break;
        }
        
    }
}