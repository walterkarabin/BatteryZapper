using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace WD.ChessMaze
{
    public class MapVisualizer : MonoBehaviour
    {
        private Transform parent;
        public Color startColor, exitColor;

        Dictionary<Vector3, GameObject> dictionaryOfObstacles = new Dictionary<Vector3, GameObject>();

        [Range(0.1f, 1f)]public float minDistance = 0.5f;
        [Range(0.1f, 1f)]public float fineDistance = 0.25f;

        private VertexPath lastPath;

        private void Awake()
        {
            parent = this.transform;
        }

        public void VisualizeMap (MapGrid grid, MapData data, bool visualizeUsingPrefabs)
        {
            if (visualizeUsingPrefabs)
            {
                
            }
            else
            {
                VisualizeUsingPrimitives(grid, data);
            }
        }

        private void VisualizeUsingPrimitives(MapGrid grid, MapData data)
        {
            PlaceStartAndExitPoints(data);
            for (int i = 0; i < data.obstacleArray.Length; i++)
            {
                if (data.obstacleArray[i])
                {
                    var positionOnGrid = grid.CalculateCoordinatesFromIndex(i);
                    if (positionOnGrid == data.startPosition || positionOnGrid == data.exitPosition)
                    {
                        continue;
                    }
                    grid.SetCell(positionOnGrid.x, positionOnGrid.z, CellObjectType.Obstacle);
                    if (PlaceKnightObstacle(data, positionOnGrid))
                    {
                        continue;
                    }
                    if (dictionaryOfObstacles.ContainsKey(positionOnGrid) == false)
                    {
                        CreateIndicator(positionOnGrid, Color.white, PrimitiveType.Cube);
                    }
                }
            }
        }

        private bool PlaceKnightObstacle(MapData data, Vector3 positionOnGrid)
        {
            foreach (var knight in data.knightPiecesList)
            {
                if (knight.Position == positionOnGrid)
                {
                    CreateIndicator(positionOnGrid, Color.red, PrimitiveType.Cube);
                    return true;
                }
            }
            return false;
        }

        private void PlaceStartAndExitPoints(MapData data)
        {
            CreateIndicator(data.startPosition, startColor, PrimitiveType.Sphere);
            CreateIndicator(data.exitPosition, exitColor, PrimitiveType.Sphere);
        }

        private void CreateIndicator(Vector3 position, Color color, PrimitiveType sphere)
        {
            var element = GameObject.CreatePrimitive(sphere);
            element.transform.position = position + new Vector3(0.5f, 1f, 0.5f);// position with the added offset to make the objects lineup with grid
            element.transform.parent = parent;
            dictionaryOfObstacles.Add(element.transform.position, element);
            var renderer = element.GetComponent<Renderer>();
            renderer.material.SetColor("_BaseColor", color);
        }

        public void ClearMap()
        {
            foreach (var obstacle in dictionaryOfObstacles.Values)
            {
                Destroy(obstacle);
            }
            dictionaryOfObstacles.Clear();
        }

        public void ClearPath(VertexPath path)
        {
            lastPath = path;
            Vector3 position;
            Dictionary<Vector3, GameObject> list = new Dictionary<Vector3, GameObject>(dictionaryOfObstacles);
            Dictionary<Vector3, GameObject> others = new Dictionary<Vector3, GameObject>(dictionaryOfObstacles);

            
            
            list.Remove(path.GetPoint(0));
            list.Remove(path.GetPoint(path.NumPoints-1));
            others.Remove(path.GetPoint(0));
            others.Remove(path.GetPoint(path.NumPoints-1));
            for (int i = 0; i < path.NumPoints; i++)
            {
                Debug.Log(list.Count);
                position = path.GetPoint(i);
                
                foreach (var obstacle in dictionaryOfObstacles.Values)
                {
                    /* if (i == 0 || i == path.NumPoints - 1)
                    {
                        Debug.Log(list.Remove(obstacle.transform.position));
                        Debug.Log(others.Remove(obstacle.transform.position));
                    } */
                    if (position.x >= obstacle.transform.position.x-minDistance && position.x <= obstacle.transform.position.x+minDistance)
                    {
                        if (position.z >= obstacle.transform.position.z-minDistance && position.z <= obstacle.transform.position.z+minDistance)
                        {
                            Debug.Log("Within box");
                            obstacle.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.black);
                            Debug.Log(list.Remove(obstacle.transform.position));
                            Debug.Log(others.Remove(obstacle.transform.position));
                        }
                    }
                }
                foreach (var obstacle in list.Values)
                {
                    if ((position.x + fineDistance >= obstacle.transform.position.x-minDistance && position.x + fineDistance <= obstacle.transform.position.x+minDistance)
                    || (position.x-fineDistance >= obstacle.transform.position.x-minDistance && position.x-fineDistance <= obstacle.transform.position.x+minDistance))
                    {
                        if ((position.z + fineDistance >= obstacle.transform.position.z-minDistance && position.z + fineDistance <= obstacle.transform.position.z+minDistance)
                        || (position.z - fineDistance >= obstacle.transform.position.z-minDistance && position.z - fineDistance <= obstacle.transform.position.z+minDistance))
                        {
                            Debug.Log("Within Buble of point");
                            obstacle.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.blue);
                            Debug.Log(others.Remove(obstacle.transform.position));
                        }
                    }
                }
                foreach (var obstacle in others.Values)
                {
                    obstacle.gameObject.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.gray);
                }
            }
            // dictionaryOfObstacles.Add(path.GetPoint(0));
            // dictionaryOfObstacles.Add(path.GetPoint(path.NumPoints-1));
            
        }
        public void RefreshPath()
        {
            ClearPath(lastPath);
        }
    }
}