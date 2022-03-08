using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WD.ChessMaze
{
    public class CandidateMap
    {
        // Start is called before the first frame update
        private MapGrid grid;

        private int numOfPieces = 0;
        private bool[] obstaclesArr = null;
        private Vector3 startPoint, exitPoint;
        private List<KnightPiece> knightPiecesList;

        public MapGrid Grid { get => grid; }
        public bool[] ObstaclesArr { get => obstaclesArr; }

        public CandidateMap(MapGrid grid, int numOfPieces)
        {
            this.grid = grid;
            this.numOfPieces = numOfPieces;
        }

        public void CreateMap(Vector3 startPoint, Vector3 exitPoint, bool autoRepair = false)
        {
            this.startPoint = startPoint;
            this.exitPoint = exitPoint;
            obstaclesArr = new bool[grid.Width*grid.Length];
            this.knightPiecesList = new List<KnightPiece>();
            RandomlyPlaceKnightPieces(this.numOfPieces);
            PlaceObstacles();
        }

        private bool CheckIfPositionCanBeObstacle(Vector3 position)
        {
            if (position == startPoint || position == exitPoint)
            {
                return false;
            }
            int index = grid.CalculateIndexFromCoordinates(position.x, position.z);

            return obstaclesArr[index] == false;
        }

        private void RandomlyPlaceKnightPieces(int numOfPieces)
        {
            var count = numOfPieces;
            var knightTryLimit = 100;
            while (count > 0 && knightTryLimit > 0)
            {
                var randomIndex = Random.Range(0, obstaclesArr.Length);
                if (obstaclesArr[randomIndex] == false)
                {
                    var coordinates = grid.CalculateCoordinatesFromIndex(randomIndex);
                    if (coordinates == startPoint || coordinates == exitPoint)
                    {
                        continue;
                    }
                    obstaclesArr[randomIndex] = true;
                    knightPiecesList.Add(new KnightPiece(coordinates));
                    count--;
                }
                knightTryLimit--;
            }
        }

        private void PlaceObstaclesForThisKnight(KnightPiece knight)
        {
            foreach (var position in KnightPiece.listOfPossibleMoves)
            {
                var newPosition = knight.Position + position;
                if (grid.IsCellValid(newPosition.x, newPosition.z) && CheckIfPositionCanBeObstacle(newPosition))
                {
                    obstaclesArr[grid.CalculateIndexFromCoordinates(newPosition.x, newPosition.z)] = true;
                }
            }
        }

        private void PlaceObstacles()
        {
            foreach (var knight in knightPiecesList)
            {
                PlaceObstaclesForThisKnight(knight);
            }
        }
        public MapData ReturnMapData()
        {
            return new MapData
            {
                obstacleArray = this.obstaclesArr,
                knightPiecesList = this.knightPiecesList,
                startPosition = startPoint,
                exitPosition = exitPoint
            };
        }
    }
}