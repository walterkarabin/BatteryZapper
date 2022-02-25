using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralFloor : MonoBehaviour
{
    [SerializeField] private GameObject floorTile;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject proceduralGeneration;

    private Vector3 playerLoc, lastLoc;
    // Start is called before the first frame update
    void Start()
    {
        lastLoc = playerLoc;
    }

    // Update is called once per frame
    void Update()
    {
        playerLoc = player.transform.position;
        if (playerLoc.x - lastLoc.x > 0.5f)
        {
            GameObject tempObj = SpawnObj(floorTile, (int)playerLoc.x + 1, 0);
            lastLoc = playerLoc;
        }
        else if (playerLoc.x - lastLoc.x < -0.5f)
        {
            GameObject tempObj = SpawnObj(floorTile, (int)playerLoc.x - 1, 0);
            lastLoc = playerLoc;
        }
    }

    GameObject SpawnObj(GameObject obj, int width, int height) // whatever is sopawned will be a child of the procedural generation game object
    {
        obj = Instantiate(obj, new Vector3(width, height, 0), Quaternion.identity);
        obj.transform.parent = proceduralGeneration.transform;
        obj.layer = proceduralGeneration.layer;
        return obj;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Object Layer: " + other.gameObject.layer + " and Name: " + other.gameObject.name);
        if (other.gameObject.layer == 3)
        {
            Debug.Log("Position: " + other.gameObject.transform.position);
            Destroy(other.gameObject);
        }
    }
}
