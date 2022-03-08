using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

namespace WD.ChessMaze
{
    public class PathMaker : MonoBehaviour
    {
        public PathCreator pathCreator;
        public MapGenerator mapGenerator;
        public GridVisualizer gridVisualizer;
        [Range(0,8)] public int numPoints;
        private Vector3 start, exit;

        private VertexPath path;

        public VertexPath Path { get => path; }

        public void CreatePath(Vector3 start, Vector3 exit, MapGrid grid, int width, int length)
        {

            Debug.Log("start: " + start + ", exit: " + exit);

            List<Vector3> points = new List<Vector3>();
            points.Add(start);
            for (int i = 0; i < numPoints; i++)
            {
                points.Add(GetRandomPointOnMap(width, length, grid));
            }
            points.Add(exit);

            BezierPath bezierPath = new BezierPath(points, false, PathSpace.xyz);
            GetComponent<PathCreator>().bezierPath = bezierPath;
            path = GetComponent<PathCreator>().path;
        }
        public Vector3 GetRandomPointOnMap(int width, int length, MapGrid grid)
        {
            int x = Random.Range(0, width);
            int z = Random.Range(0, length);

            Vector3 pos =  new Vector3(x, 1f, z);

            if (grid.IsCellValid(x,z))
            {
                return pos;
            }
            else
            {
                Debug.Log("No point found!");
                GetRandomPointOnMap(width, length, grid);
            }
            return new Vector3(0, 0, 0);
        }
    }
}