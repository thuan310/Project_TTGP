using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class PositionGuilding : MonoBehaviour
{

    public static PositionGuilding instance;
    public PlayerManager player;
    public NavMeshAgent agent;
    //public Camera camera;
    [SerializeField] GameObject sparklePrefab; // Prefab for the sparkle (e.g., a small sphere or particle system)
    [SerializeField] Transform destinationPosition;

    [Header("PathGuildingSettings")]
    [SerializeField] float sparkleSpacing = 0.5f; // Distance between sparkles
    [SerializeField] bool isGuilding = false;

    public void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    CreateSparklesAlongPath();
    //}

    void CreateSparklesAlongPath(Vector3[] corners)
    {
        UpdateSparkles();
        // Loop through the positions and create sparkles
        for (int i = 0; i < corners.Length - 1; i++)
        {
            Vector3 start = corners[i];
            Vector3 end = corners[i + 1];

            // Calculate the distance between the current and next point
            float distance = Vector3.Distance(start, end);

            // Calculate the number of sparkles to place between these two points
            int numSparkles = Mathf.FloorToInt(distance / sparkleSpacing);

            // Create sparkles along the line segment
            for (int j = 0; j < numSparkles; j++)
            {
                float t = (float)j / numSparkles;
                Vector3 sparklePosition = Vector3.Lerp(start, end, t);

                // Instantiate the sparkle at the calculated position
                Instantiate(sparklePrefab, sparklePosition, Quaternion.identity, transform);
            }
        }
    }

    void UpdateSparkles()
    {
        // Destroy existing sparkles in the child of PositioinGuiding object
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetDestinationForGuilding(Transform destination, bool isGuilding )
    {
        destinationPosition = destination;
        this.isGuilding = isGuilding; 
    }
    // Update is called once per frame
    void Update()
    {
        if(!isGuilding)
        {
            UpdateSparkles();
            return;
        }

        int layerMask = ~(1 << LayerMask.NameToLayer("Ignore Raycast")); // Ignore a specific layer
        if (Physics.Raycast(destinationPosition.position, Vector3.down, out RaycastHit hitInfo,100,layerMask))
        {
            Debug.DrawRay(destinationPosition.position, Vector3.down * 100, Color.red, 2f);
            NavMeshPath path = new NavMeshPath();
            //agent.SetDestination(hitInfo.point);
            agent.CalculatePath(hitInfo.point, path);

            Vector3[] corners = path.corners;

            //print(path.corners.Length);
            // kiểm tra vị trí các góc rẽ
            //Debug.Log("Path has " + corners.Length + " corners:");
            //for (int i = 0; i < corners.Length; i++)
            //{
            //    Debug.Log("Corner " + i + ": " + corners[i]);
            //}

            // Tạo sparkles
            CreateSparklesAlongPath(corners);


        }
    }
}
