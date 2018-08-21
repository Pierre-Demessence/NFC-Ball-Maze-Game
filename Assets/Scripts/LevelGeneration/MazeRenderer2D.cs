using System.Collections.Generic;
using LevelGeneration.Interfaces;
using UnityEngine;

namespace LevelGeneration
{
    public class MazeRenderer2D : MonoBehaviour, IMazeRenderer<Level2D>
    {
        [SerializeField] private Vector3 _cellSize = Vector3.one * 1.5f;
        [SerializeField] private float _wallThickness = 0.1f;

        public void Render(Level2D level)
        {
        }

        private List<Vector3> GetWallVerts(CellData2D cell)
        {
            var verts = new List<Vector3>();
            var size = _cellSize / 2;

            if (cell.NorthWall)
            {
                verts.Add(new Vector3(-size.x + _wallThickness, 0, size.z - _wallThickness));
                verts.Add(new Vector3(-size.x + _wallThickness, _cellSize.y, size.z - _wallThickness));
                verts.Add(new Vector3(-size.x + _wallThickness, _cellSize.y, size.z));

                verts.Add(new Vector3(size.x - _wallThickness, _cellSize.y, size.z));
                verts.Add(new Vector3(size.x - _wallThickness, _cellSize.y, size.z - _wallThickness));
                verts.Add(new Vector3(size.x - _wallThickness, 0, size.z - _wallThickness));
            }
            
            if (cell.EastWall)
            {
                verts.Add(new Vector3(size.x - _wallThickness, 0, size.z - _wallThickness));
                verts.Add(new Vector3(size.x - _wallThickness, _cellSize.y, size.z - _wallThickness));
                verts.Add(new Vector3(size.x, _cellSize.y, size.z - _wallThickness));
                
                verts.Add(new Vector3(size.x, _cellSize.y, -size.z + _wallThickness));
                verts.Add(new Vector3(size.x - _wallThickness, _cellSize.y, -size.z + _wallThickness));
                verts.Add(new Vector3(size.x - _wallThickness, 0, -size.z + _wallThickness));

            }
        }

        private void MakeWall(List<Vector3> verts, Vector3 size)
        {
            verts.Add(new Vector3(-size.x + _wallThickness, 0, size.z));
            verts.Add(new Vector3(-size.x + _wallThickness, _cellSize.y, size.z));
            verts.Add(new Vector3(-size.x + _wallThickness, _cellSize.y, size.z + _wallThickness));

            verts.Add(new Vector3(size.x - _wallThickness, _cellSize.y, size.z + _wallThickness));
            verts.Add(new Vector3(size.x - _wallThickness, _cellSize.y, size.z));
            verts.Add(new Vector3(size.x - _wallThickness, 0, size.z));
        }
    }
}