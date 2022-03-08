using System.Collections.Generic;
using UnityEngine;

namespace WD.ChessMaze
{
    public struct MapData
    {
        public bool[] obstacleArray;
        public List<KnightPiece> knightPiecesList;
        public Vector3 startPosition;
        public Vector3 exitPosition;
    }
}