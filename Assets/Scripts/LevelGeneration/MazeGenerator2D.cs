using System.Collections.Generic;
using System.Linq;
using LevelGeneration.Interfaces;
using UnityEngine;

namespace LevelGeneration
{
    public class MazeGenerator2D : IMazeGenerator<Level2D>
    {
        private readonly int _width, _height;
        private readonly Dictionary<Vector2Int, CellData2D> _visitedCells;
        private readonly Dictionary<Vector2Int, CellData2D> _unvisitedCells;

        public MazeGenerator2D(int width, int height)
        {
            _width = width;
            _height = height;
            _visitedCells = new Dictionary<Vector2Int, CellData2D>();
            _unvisitedCells = new Dictionary<Vector2Int, CellData2D>();
        }

        public Level2D Generate()
        {
            Initialize();

            var curr = _unvisitedCells.Values.First();
            var prev = new CellData2D(new Vector2Int(-1, -1));
            Random rand = new Random();

            while (_unvisitedCells.Count > 0)
            {
                _unvisitedCells.Remove(curr.Position);
                _visitedCells.Add(curr.Position, curr);

                if (prev.Position != new Vector2Int(-1, -1))
                {
                    curr.RemoveWall(prev.Position);
                }

                if (HasUnvisitedNeighbours(curr.Position))
                {
                    var next = _unvisitedCells[GetRandomUnvisitedNeighbour(curr.Position)];
                    curr.RemoveWall(next.Position);
                    prev = curr;
                    curr = next;
                }
                else if (_unvisitedCells.Count > 0)
                {
                    var unvisitedCell = _unvisitedCells.Values.First();

                    prev = _visitedCells[GetRandomVisitedNeighbour(unvisitedCell.Position)];
                    prev.RemoveWall(unvisitedCell.Position);
                    curr = unvisitedCell;
                }
            }

            return new Level2D {Data = _visitedCells};
        }

        private void Initialize()
        {
            Vector2Int positionIndex = Vector2Int.zero;
            for (int i = 0; i < _width * _height; i++)
            {
                _unvisitedCells.Add(positionIndex, new CellData2D(positionIndex));
                if (positionIndex.x == _width - 1)
                {
                    positionIndex.x = 0;
                    positionIndex.y++;
                }
                else positionIndex.x++;
            }
        }

        private Vector2Int GetRandomVisitedNeighbour(Vector2Int position)
        {
            var neighbours = GetNeighbours(position);

            foreach (Vector2Int neighbour in GetNeighbours(position))
            {
                if (_visitedCells.ContainsKey(neighbour)) neighbours.Remove(neighbour);
            }

            return neighbours[Random.Range(0, neighbours.Count)];
        }

        private Vector2Int GetRandomUnvisitedNeighbour(Vector2Int position)
        {
            var neighbours = GetNeighbours(position);

            foreach (Vector2Int neighbour in GetNeighbours(position))
            {
                if (_unvisitedCells.ContainsKey(neighbour)) neighbours.Remove(neighbour);
            }

            return neighbours[Random.Range(0, neighbours.Count)];
        }

        private List<Vector2Int> GetNeighbours(Vector2Int position)
        {
            var neighbours = new List<Vector2Int>();

            var left = position + Vector2Int.left;
            if (left.x >= 0) neighbours.Add(left);

            var right = position + Vector2Int.right;
            if (right.x < _width) neighbours.Add(right);

            var top = position + Vector2Int.up;
            if (top.y < _height) neighbours.Add(top);

            var down = position + Vector2Int.down;
            if (down.y >= 0) neighbours.Add(down);

            return neighbours;
        }

        private bool HasUnvisitedNeighbours(Vector2Int position)
        {
            foreach (Vector2Int neighbour in GetNeighbours(position))
            {
                return _unvisitedCells.ContainsKey(neighbour);
            }

            return false;
        }
    }
}