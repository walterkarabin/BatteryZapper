using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WD.ChessMaze
{
    public class MapGenerator : MonoBehaviour
    {
        public GridVisualizer gridVisualizer;
        public MapVisualizer mapVisualizer;
        public PathMaker pathMaker;
        public Direction startEdge, exitEdge;
        public bool randomPlacement;
        [Range(1,10)] public int numOfPieces;
        private Vector3 startPosition, exitPosition;

        [Range(3,20)] public int width, length = 11;
        private MapGrid grid;

        public Vector3 GetStartPosition { get => startPosition; }
        public Vector3 GetExitPosition { get => exitPosition; }

        // Start is called before the first frame update
        void Start()
        {
            gridVisualizer.VisualizeGrid(width, length);
            GenerateNewMap();
        }

        public void GenerateNewMap()
        {
            gridVisualizer.VisualizeGrid(width, length);
            mapVisualizer.ClearMap();


            grid = new MapGrid(width, length);

            MapHelper.RandomlyChooseAndSetStartAndExit(grid, ref startPosition, ref exitPosition, randomPlacement, startEdge, exitEdge);
            pathMaker.CreatePath(startPosition + new Vector3(0.5f, 1f, 0.5f), exitPosition + new Vector3(0.5f, 1f, 0.5f), grid, width, length);


            CandidateMap map = new CandidateMap(grid, numOfPieces);
            map.CreateMap(startPosition, exitPosition);
            mapVisualizer.VisualizeMap(grid, map.ReturnMapData(), false);
            ClearPath();
        }

        public void ClearPath()
        {
            mapVisualizer.ClearPath(pathMaker.Path);
        }

        

        // Update is called once per frame
        void Update()
        {

        }
    }
}