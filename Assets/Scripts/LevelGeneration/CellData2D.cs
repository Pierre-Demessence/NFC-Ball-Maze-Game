using UnityEngine;

namespace LevelGeneration
{
    public class CellData2D
    {
        public readonly Vector2Int Position;
        public object UserData;

        private bool _northWall, _southWall, _eastWall, _westWall;
        public bool NorthWall => _northWall;
        public bool SouthWall => _southWall;
        public bool EastWall => _eastWall;
        public bool WestWall => _westWall;

        public CellData2D(Vector2Int position)
        {
            Position = position;
            _northWall = _southWall = _eastWall = _westWall = true;
        }

        public CellData2D(Vector2Int position, object userData) : this(position)
        {
            UserData = userData;
        }

        public void RemoveWall(Vector2Int dir)
        {
            if (dir.x > Position.x)
                _eastWall = false;
            else if (dir.x < Position.x)
                _westWall = false;
            else if (dir.y > Position.y)
                _northWall = false;
            else if (dir.y < Position.y)
                _southWall = false;
        }
    }
}