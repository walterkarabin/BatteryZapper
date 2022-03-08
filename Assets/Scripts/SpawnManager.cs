using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private float maxDistance = 7.5f;
    private float maxObstacleDistance = 5f;
    private Vector3 center;
    public Transform level;

    // Reference to the player gameObject
    public GameObject player;

    //For instantiation of prefabs
    public EnemyAI enemy;
    public Battery battery;
    public GameObject obstacle;

    // int to keep track of Enemies in the game
    private int numOfEnemies = 0;
    // int to keep track of batteries in the game
    private int numOfBatteries = 0;
    // Maximum Number of Enemies
    public int maxEnemies;
    // Maximum Number of Batteries
    public int maxBatteries;
    // Maximum Number of Obstacles
    public int maxObstacles;
    // Keep track of enemies killed by player
    private int numEnemiesKilled = 0;
    private int waveCounter = 0;
    // Keep track of all charged batteries
    private int numBatteriesCharged = 0;
    // Transforms for organization
    private Transform parent;
    [SerializeField]private Transform ObstacleParent;
    [SerializeField]private Transform BatteryParent;
    [SerializeField]private Transform EnemyParent;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        center = level.position;
        parent = this.transform;
        ObstacleParent.parent = parent;
        BatteryParent.parent = parent;
        EnemyParent.parent = parent;

        SpawnObstacles();
        SpawnBatteries();
        SpawnEnemies();
    }

    private void OnEnable()
    {
        EnemyAI.OnKilled += KillCounter;
        Battery.OnFullyCharged += ChargedBattery;
        Battery.OnNotFullyCharged += UnChargedBattery;
    }

    private void OnDisable()
    {
        EnemyAI.OnKilled -= KillCounter;
        Battery.OnFullyCharged += ChargedBattery;
        Battery.OnNotFullyCharged += UnChargedBattery;
    }

    // Update is called once per frame
    // * constantly checking if there are no more enemies, if true then spawn them
    void Update()
    {
        if (numOfEnemies == 0 && waveCounter < 3)
        {
            Invoke("SpawnEnemies", 2f);
        }

    }
    // ! Finding points on the NavMesh ! // ----------------------------------------------------
    public Vector3 GetRandomPointOnMap()
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxDistance + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        bool foundPosition = NavMesh.SamplePosition(randomPos, out hit, maxDistance, NavMesh.AllAreas);
        if (foundPosition)
        {
            return hit.position;
        }
        else
        {
            // Look again for valid point on NavMesh surface
            return GetRandomPointOnMap();
        }

    }
    public Vector3 GetRandomPointOnMap(float maxRadius)
    {
        // Get Random Point inside Sphere which position is center, radius is maxDistance
        Vector3 randomPos = Random.insideUnitSphere * maxRadius + center;

        NavMeshHit hit; // NavMesh Sampling Info Container

        // from randomPos find a nearest point on NavMesh surface in range of maxDistance
        bool foundPosition = NavMesh.SamplePosition(randomPos, out hit, maxRadius, NavMesh.AllAreas);
        if (foundPosition)
        {
            return hit.position;
        }
        else
        {
            // Look again for valid point on NavMesh surface
            return GetRandomPointOnMap(maxRadius);
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
            return hit.position;
        }
        else
        {
            // Look again for valid point on NavMesh surface
            return GetRandomPointFromPlayer();
        }

    }
    // ! Spawning Enenmies ! // ----------------------------------------------------------------
    public void SpawnEnemies()
    {
        if (numOfEnemies == 0)
        {
            for (int i = 0; i < maxEnemies; i++)
            {
                SpawnEnemy();
            }
            waveCounter++;
        }
        else if (numOfEnemies < maxEnemies)
        {
            for (int i = 0; i < (maxEnemies - numOfEnemies); i++)
            {
                SpawnEnemy();
            }
            waveCounter++;
        }
        else
        {
            Debug.Log("Number of enemies is: " + numOfEnemies);
        }
    }
    public void SpawnEnemy()
    {
        Vector3 point = new Vector3();
        point = GetRandomPointFromPlayer();
        point.x = Mathf.Round(point.x * 100f) / 100f;
        point.z = Mathf.Round(point.z * 100f) / 100f;

        var enemyRef = Instantiate<EnemyAI>(enemy, point, Quaternion.identity);
        enemyRef.transform.parent = EnemyParent;
        numOfEnemies++;
    }

    // ! Spawning Batteries ! // ----------------------------------------------------------------
   public void SpawnBatteries()
    {
        if (numOfBatteries == 0)
        {
            for (int i = 0; i < maxBatteries; i++)
            {
                SpawnBattery();
            }
        }
        else if (numOfBatteries < maxBatteries)
        {
            for (int i = maxBatteries - numOfBatteries; i < maxBatteries; i++)
            {
                SpawnBattery();
            }
        }
        else
        {
            Debug.Log("Number of batteries is: " + numOfBatteries);
        }
    }
    public void SpawnBattery()
    {
        Vector3 point = new Vector3();
        point = GetRandomPointOnMap() + new Vector3(0, .25f, 0);
        point.x = Mathf.Round(point.x * 100f) / 100f;
        point.z = Mathf.Round(point.z * 100f) / 100f;

        var battRef = Instantiate<Battery>(battery, point, Quaternion.identity);
        battRef.transform.parent = BatteryParent;
        numOfBatteries++;
    }
    // ! Spawning Obstacles ! // ----------------------------------------------------------------
    public void SpawnObstacles()
    {
        for (int i = 0; i < maxObstacles; i++)
        {
            SpawnObstacle();
        }
    }
    public void SpawnObstacle()
    {
        Vector3 point = new Vector3();
        point = GetRandomPointOnMap(maxObstacleDistance) + new Vector3(0, .15f, 0);
        point.x = Mathf.Round(point.x * 100f) / 100f;
        point.z = Mathf.Round(point.z * 100f) / 100f;

        var obstacleRef = Instantiate<GameObject>(obstacle, point, Quaternion.identity);
        obstacleRef.transform.parent = ObstacleParent;
    }
    public void KillCounter()
    {
        numEnemiesKilled++;
        numOfEnemies--;
    }

    public void ChargedBattery()
    {
        if (numBatteriesCharged < maxBatteries)
        {
            numBatteriesCharged++;
            numOfBatteries--;
        }
        if (numBatteriesCharged == maxBatteries)
        {
            // Add Event for ending this level
            Debug.Log("ALL BATTERIES ARE CHARGED");
        }
    }
    public void UnChargedBattery()
    {
        if (numBatteriesCharged > 0)
        {
            numBatteriesCharged--;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, maxDistance);
    }
}
