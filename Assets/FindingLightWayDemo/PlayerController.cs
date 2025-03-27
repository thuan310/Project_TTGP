using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

public class PlayerController : MonoBehaviour
{
    public NavMeshAgent agent;
    //public Camera camera;
    public GameObject sparklePrefab; // Prefab for the sparkle (e.g., a small sphere or particle system)
    public float sparkleSpacing = 0.5f; // Distance between sparkles

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
        // Destroy existing sparkles
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(movePosition, out var hitInfo))
            {
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
}
