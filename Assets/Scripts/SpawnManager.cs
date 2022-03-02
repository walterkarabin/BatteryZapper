using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private float maxDistance = 5f;
    private Vector3 center;
    public Transform level;

    // Reference to the player gameObject
    public GameObject player;

    //For instantiation of prefabs
    public EnemyAI enemy;
    public Battery battery;

    //list to keep track of Enemies
    private List<EnemyAI> numOfEnemies = new List<EnemyAI>();
    // list to keep track of batteries
    private List<Battery> numOfBatteries = new List<Battery>();
    // Maximum Number of Enemies
    public int maxEnemies;
    // Maximum Number of Batteries
    public int maxBatteries;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        center = level.position;

        SpawnBatteries();
        SpawnEnemies();
    }

    private void OnEnable() {
        // EnemyAI.OnKilled += Spawn;
    }

    private void OnDisable() {
        // EnemyAI.OnKilled -= Spawn;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public Vector3 GetRandomPointOnMap()
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
    public Vector3 GetRandomPointFromPlayer()
    {
        // Get Random Point inside Sphere which position is player, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + player.transform.position;

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
            return player.transform.position;
        }

    }
    public void SpawnEnemies()
    {
        numOfEnemies = new List<EnemyAI>(GameObject.FindObjectsOfType<EnemyAI>());
        if (numOfEnemies.Count == 0)
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                SpawnEnemy();
            }
        }
        else if (numOfEnemies.Count < maxEnemies)
        {
            for (int i = 0; i < (maxEnemies - numOfEnemies.Count); i++)
            {
                SpawnEnemy();
                Debug.Log(i);
            }
        }
        else
        {
            Debug.Log("Number of enemies is: " + numOfEnemies.Count);
        }
    }
    public void SpawnEnemy()
    {
        Vector3 point = new Vector3();
        point = GetRandomPointFromPlayer();

        EnemyAI enemyRef = Instantiate<EnemyAI>(enemy, point, Quaternion.identity);
        numOfEnemies.Add(enemyRef);

        Debug.Log("Number of enemies is: " + numOfEnemies.Count);
    }

   public void SpawnBatteries()
    {
        numOfBatteries = new List<Battery>(GameObject.FindObjectsOfType<Battery>());
        if (numOfBatteries.Count == 0)
        {
            for (int i = 0; i < maxBatteries; i++)
            {
                SpawnBattery();
            }
        }
        else if (numOfBatteries.Count < maxBatteries)
        {
            for (int i = (maxBatteries - numOfBatteries.Count); i < maxBatteries; i++)
            {
                SpawnBattery();
                Debug.Log(i);
            }
        }
        else
        {
            Debug.Log("Number of batteries is: " + numOfBatteries.Count);
        }
    }
    public void SpawnBattery()
    {
        Vector3 point = new Vector3();
        point = GetRandomPointOnMap();

        Battery batteryRef = Instantiate<Battery>(battery, point, Quaternion.identity);
        numOfBatteries.Add(batteryRef);

        Debug.Log("Number of Batteries is: " + numOfBatteries.Count);
    }

    public void UpdateLists() {
        numOfEnemies = new List<EnemyAI>(GameObject.FindObjectsOfType<EnemyAI>());
        numOfBatteries = new List<Battery>(GameObject.FindObjectsOfType<Battery>());
    }
}
