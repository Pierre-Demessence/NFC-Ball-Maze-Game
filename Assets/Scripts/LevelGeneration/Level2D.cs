using System.Collections.Generic;
using LevelGeneration.Interfaces;
using UnityEngine;

namespace LevelGeneration
{
    public class Level2D : ILevel
    {
        public Vector2Int Size;
        public Vector2Int StartPosition;
        public Vector2Int EndPositon;
        public Dictionary<Vector2Int, CellData2D> Data;

        public static void SerializeTo(Level2D level, string path)
        {
        }

        public static Level2D DeserializeFrom(string path)
        {
            return null;
        }
    }
}