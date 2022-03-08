using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace WD.ChessMaze
{
    public class GridVisualizer : MonoBehaviour
    {
        public GameObject groundPrefab;
        public GameObject wallPrefab;
        [Range(1, 3)] public int wallHeight;
        private GameObject board;
        private GameObject wall;
        private Transform parent;
        Dictionary<Vector3, GameObject> dictionaryOfGrid = new Dictionary<Vector3, GameObject>();

        public Transform Parent { get => parent; }

        private void Awake()
        {
            parent = this.transform;
        }

        public void VisualizeGrid(int width, int length)
        {
            ClearGrid();
            //Use individual tiles for grid layout, that way we can change the grid pattern (Potentially make holes in the grid)
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    CreateFloor(i, j, groundPrefab);
                }
            }
            CreateWalls(width, length, wallPrefab, wallHeight);

            
        }

        private void CreateFloor(int x, int z, GameObject prefab)
        {
            Vector3 pos = new Vector3(x, 0, z) + new Vector3(0.5f, 0f, 0.5f);// position includes offset
            board = Instantiate(prefab, pos, Quaternion.identity);
            board.transform.parent = Parent;
            dictionaryOfGrid.Add(pos, board);
        }

        public void ClearGrid()
        {
            foreach (var ground in dictionaryOfGrid.Values)
            {
                Destroy(ground);
            }
            dictionaryOfGrid.Clear();
        }

        private void CreateWallBlock(int x, int z, GameObject prefab, int wallHeight)
        {
            for (int i = 0; i < wallHeight; i++)
            {
                Vector3 pos = new Vector3(x, 0, z) + new Vector3(0.5f, i, 0.5f);
                wall = Instantiate(prefab, pos, Quaternion.identity);
                wall.transform.parent = Parent;
                dictionaryOfGrid.Add(pos, wall);
            }
        }
        private void CreateWalls(int width, int length, GameObject prefab, int wallHeight)
        {
            for (int i = -1; i < width + 1; i++)
            {
                CreateWallBlock(i, -1, prefab, wallHeight);
                CreateWallBlock(i, length, prefab, wallHeight);
            }
            for (int i = 0; i < length; i++)
            {
                CreateWallBlock(-1, i, prefab, wallHeight);
                CreateWallBlock(width, i, prefab, wallHeight);
            }
        }
    }
}

