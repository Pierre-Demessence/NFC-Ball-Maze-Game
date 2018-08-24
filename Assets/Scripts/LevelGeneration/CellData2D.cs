using System;
using UnityEngine;

namespace LevelGeneration
{
    [Serializable]
    public class CellData2D
    {
        public Vector2Int Position;
        public object UserData;

        public bool SouthWall, WestWall;

        public CellData2D(Vector2Int position)
        {
            Position = position;
            SouthWall = WestWall = true;
        }

        public CellData2D(Vector2Int position, object userData) : this(position)
        {
            UserData = userData;
        }

        public void RemoveWall(Vector2Int dir)
        {
            if (dir.x < Position.x)
                WestWall = false;
            else if (dir.y < Position.y)
                SouthWall = false;
        }
    }
}