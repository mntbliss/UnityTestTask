using System.Collections.Generic;
using UnityEngine;

namespace _Bludoku.Scripts.Boards
{
    public class ClearResult
    {
        public int PlaceX;
        public int PlaceY;
        public int FigureId;
        public int ClearedCount;
        public int FiguresRemovedCount;
        public List<Vector3> ClearedPositions = new List<Vector3>();
    }
}
