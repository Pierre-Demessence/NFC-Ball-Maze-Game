﻿using NaughtyAttributes;
using UnityEngine;
using Utilities;

namespace LevelGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private int _width, _height;
        [SerializeField] private Level2D _currentMaze;
        private bool _autoPreview;

        [Button("Generate Maze")]
        private void Generate()
        {
            var mazeGenerator = new MazeGenerator2D(_width, _height);
            _currentMaze = mazeGenerator.Generate();
        }

        [Button("Preview Maze")]
        private void Preview()
        {
            LevelMeshGenerator2D gen = gameObject.GetOrAddComponent<LevelMeshGenerator2D>();
            gen.Render(_currentMaze);
        }
    }
}