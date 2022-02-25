using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private float maxDistance = 5f;
    private Vector3 center;
    public Transform level;

    private Battery battery;

    // Start is called before the first frame update
    void Start()
    {
        center = level.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetRandomPoint()
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        bool foundPosition = NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
        if (foundPosition)
        {
            Debug.Log("Found");
            return hit.position;
        }
        else
        {
            Debug.Log("Not Found");
            return center;
        }

    }
    public void Spawn(int num)
    {
        if (num is 1)
        {
            Debug.Log("num is 1");
        }
        else
        {
            Debug.Log("num is " + num);
        }
    }
}
