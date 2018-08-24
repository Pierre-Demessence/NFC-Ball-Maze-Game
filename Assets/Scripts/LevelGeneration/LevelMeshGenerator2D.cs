using System;
using System.Collections.Generic;
using LevelGeneration.Interfaces;
using UnityEngine;
using Utilities;

namespace LevelGeneration
{
    public class LevelMeshGenerator2D : MonoBehaviour, IMeshGenerator<Level2D>
    {
        [Flags]
        private enum WallFace
        {
            Forward = 0,
            Backward = 1,
            Left = 2,
            Right = 4
        }

        [SerializeField] private Vector3 _cellSize = Vector3.one * 1.5f;
        [SerializeField] private float _wallThickness = 0.2f;
        [SerializeField] private Level2D _currentLevel;

        public void Render(Level2D level)
        {
            _currentLevel = level;

            MeshFilter filter = gameObject.GetOrAddComponent<MeshFilter>();

            List<Vector3> verts = new List<Vector3>();
            List<int> triIndices = new List<int>();

            foreach (CellData2D cell in level.Data.Values)
            {
                var vertCount = verts.Count;
                var offsetX = level.Size.x * _cellSize.x * 0.5f - _cellSize.x * 0.5f;
                var offsetY = level.Size.y * _cellSize.y * 0.5f - _cellSize.y * 0.5f;
                var cellPosition = new Vector3(cell.Position.x * _cellSize.x - offsetX, 0,
                    cell.Position.y * _cellSize.y - offsetY);
                
                AddCellWallVerts(cell, cellPosition, ref verts);

                var newVertCount = verts.Count - vertCount;
                AddCellTriIndices(cell, vertCount, newVertCount, ref triIndices);
            }

            var mesh = new Mesh();
            mesh.SetVertices(verts);
            mesh.SetTriangles(triIndices, 0);
            mesh.RecalculateNormals();
            filter.mesh = mesh;

            MeshRenderer rend = gameObject.GetOrAddComponent<MeshRenderer>();
            rend.material = rend.material == null ? new Material(Shader.Find("Standard")) : rend.material;
        }

        private void AddCellWallVerts(CellData2D cell, Vector3 cellPosition, ref List<Vector3> verts)
        {
            var size = _cellSize / 2;


            if (cell.SouthWall && !cell.WestWall)
            {
                var center = cellPosition + new Vector3(0, size.y, -size.z + _wallThickness / 2);
                var boundSize = new Vector3(_cellSize.x, _cellSize.y, _wallThickness);
                var faces = WallFace.Backward | WallFace.Forward;

                var leftNeighbour = GetNeighbour(cell.Position, Vector2Int.left);
                if (leftNeighbour != null && !leftNeighbour.SouthWall) faces = faces | WallFace.Left;

                var rightNeighbour = GetNeighbour(cell.Position, Vector2Int.right);
                if (rightNeighbour != null && !rightNeighbour.SouthWall && !rightNeighbour.WestWall)
                {
                    faces = faces | WallFace.Right;
                    center.x += _wallThickness / 2;
                    boundSize.x += _wallThickness;
                }

                AddWallVerts(new Bounds(center, boundSize), faces, ref verts);
            }
            else if (cell.WestWall && !cell.SouthWall)
            {
                var center = cellPosition + new Vector3(-size.x + _wallThickness / 2, size.y, 0);
                var boundSize = new Vector3(_wallThickness, _cellSize.y, _cellSize.z);
                var faces = WallFace.Right | WallFace.Left;

                var topNeighbour = GetNeighbour(cell.Position, Vector2Int.up);
                if (topNeighbour != null && !topNeighbour.WestWall && !topNeighbour.SouthWall)
                    faces = faces | WallFace.Forward;

                var bottomNeighbour = GetNeighbour(cell.Position, Vector2Int.down);
                if (bottomNeighbour != null && !bottomNeighbour.WestWall)
                    faces = faces | WallFace.Backward;

                AddWallVerts(new Bounds(center, boundSize), faces, ref verts);
            }
            else if (cell.WestWall && cell.SouthWall)
            {
                // SouthWall
                var center = cellPosition + new Vector3(0, size.y, -size.z + _wallThickness / 2);
                var boundSize = new Vector3(_cellSize.x, _cellSize.y, _wallThickness);
                var faces = WallFace.Backward | WallFace.Forward;

                var leftNeighbour = GetNeighbour(cell.Position, Vector2Int.left);
                if (leftNeighbour != null && !leftNeighbour.SouthWall) faces = faces | WallFace.Left;

                var rightNeighbour = GetNeighbour(cell.Position, Vector2Int.right);
                if (rightNeighbour != null && !rightNeighbour.SouthWall && !rightNeighbour.WestWall)
                {
                    faces = faces | WallFace.Right;
                    center.x += _wallThickness / 2;
                    boundSize.x += _wallThickness;
                }

                AddWallVerts(new Bounds(center, boundSize), faces, ref verts);

                // WestWall
                center = cellPosition + new Vector3(-size.x + _wallThickness / 2, size.y, _wallThickness / 2);
                boundSize = new Vector3(_wallThickness, _cellSize.y, _cellSize.z - _wallThickness);
                faces = WallFace.Right | WallFace.Left;

                var topNeighbour = GetNeighbour(cell.Position, Vector2Int.up);
                if (topNeighbour != null && !topNeighbour.WestWall && !topNeighbour.SouthWall)
                    faces = faces | WallFace.Forward;

                AddWallVerts(new Bounds(center, boundSize), faces, ref verts);
            }
        }

        private void AddCellTriIndices(CellData2D cell, int offset, int vertCount, ref List<int> tris)
        {
            for (int i = 0; i < vertCount; i += 4)
            {
                tris.Add(offset + i + 0);
                tris.Add(offset + i + 1);
                tris.Add(offset + i + 2);

                tris.Add(offset + i + 2);
                tris.Add(offset + i + 3);
                tris.Add(offset + i + 0);
            }
        }

        private void AddWallVerts(Bounds bounds, WallFace faces, ref List<Vector3> verts)
        {
            // -- Top face
            verts.Add(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)); // (0, 1, 0)
            verts.Add(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)); // (0, 1, 1)
            verts.Add(bounds.max); // (1, 1, 1)
            verts.Add(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)); // (1, 1, 0)

            if ((faces & WallFace.Backward) == WallFace.Backward)
            {
                // -- Back face
                verts.Add(bounds.min); // (0, 0, 0)
                verts.Add(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)); // (0, 1, 0)
                verts.Add(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)); // (1, 1, 0)
                verts.Add(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z)); // (1, 0, 0)
            }

            if ((faces & WallFace.Forward) == WallFace.Forward)
            {
                // -- Forward face
                verts.Add(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)); // (1, 0, 1)
                verts.Add(bounds.max); // (1, 1, 1)
                verts.Add(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)); // (0, 1, 1)
                verts.Add(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z)); // (0, 0, 1)
            }

            if ((faces & WallFace.Left) == WallFace.Left)
            {
                // -- Left Face
                verts.Add(new Vector3(bounds.min.x, bounds.min.y, bounds.max.z)); // (0, 0, 1)
                verts.Add(new Vector3(bounds.min.x, bounds.max.y, bounds.max.z)); // (0, 1, 1)
                verts.Add(new Vector3(bounds.min.x, bounds.max.y, bounds.min.z)); // (0, 1, 0)
                verts.Add(bounds.min); // (0, 0, 0)
            }

            if ((faces & WallFace.Right) == WallFace.Right)
            {
                // -- Right Face
                verts.Add(new Vector3(bounds.max.x, bounds.min.y, bounds.min.z)); // (1, 0, 0)
                verts.Add(new Vector3(bounds.max.x, bounds.max.y, bounds.min.z)); // (1, 1, 0)
                verts.Add(bounds.max); // (1, 1, 1)
                verts.Add(new Vector3(bounds.max.x, bounds.min.y, bounds.max.z)); // (1, 0, 1)
            }
        }

        private CellData2D GetNeighbour(Vector2Int position, Vector2Int dir)
        {
            CellData2D cell;
            if (_currentLevel.Data.TryGetValue(position + dir, out cell)) return cell;
            return null;
        }
    }
}