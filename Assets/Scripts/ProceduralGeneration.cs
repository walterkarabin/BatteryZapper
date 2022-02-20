using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] private int start, width, range, gap;
    [SerializeField] private GameObject floorTile, roofTile;

    // Start is called before the first frame update
    void Start()
    {
        Generation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Generation()
    {
        int height = range;
        int min = height - 1;
        int max = height + 2;


        for (int x = start; x < start + width; x++)//this will spawn a tile on the x-axis
        {
            min = height - 1;
            max = height + 2;
            height = Random.Range(min, max);
            //Debug.Log("Height: " + height);

            for (int y = 0; y < height; y++)
            {
                SpawnObj(floorTile, x, y);

            }

            for (int y = height + gap; y < height+range+gap; y++)
            {
                SpawnObj(roofTile, x, y);
            }

        }
    }

    void SpawnObj(GameObject obj, int width, int height) // whatever is sopawned will be a child of the procedural generation game object
    {
        obj = Instantiate(obj, new Vector3(width, height, 0), Quaternion.identity);
        obj.transform.parent = this.transform;
    }

    private void OnBecameInvisible()
    {
        Destroy(floorTile);
        Debug.Log("Destroyed: " + floorTile);
        Destroy(roofTile);
    }
}
