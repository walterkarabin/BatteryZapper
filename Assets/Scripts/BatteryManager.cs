using UnityEngine;

public class BatteryManager : MonoBehaviour
{

    public Battery batt;
    public GameObject spawner;
    public SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        //spawnManager = GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void SpawnBattery()
    {
        Instantiate<Battery>(batt, spawnManager.GetRandomPointOnMap(), Quaternion.identity);
    }
}
