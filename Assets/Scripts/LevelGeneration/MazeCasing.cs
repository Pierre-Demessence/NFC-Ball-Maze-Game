using UnityEngine;

namespace LevelGeneration
{
    public class MazeCasing : MonoBehaviour
    {
        [SerializeField] private Vector3 _mazeSize, _pinPadding;
        [SerializeField] private float _wallThickness, _glassThickness, _floorThickness;
        [SerializeField] private Transform _northWall, _eastWall, _westWall, _southWall;
        [SerializeField] private Transform _floor, _glass, _nePin, _nwPin, _sePin, _swPin;

        public void SetSize(Vector2 size)
        {
            _mazeSize = size;
            UpdateSize();
        }

        private void OnDrawGizmosSelected()
        {
            UpdateSize();
        }

        private void UpdateSize()
        {
            Vector2 wallBounds = new Vector2(_mazeSize.x * 0.5f - _wallThickness * 0.5f,
                _mazeSize.z * 0.5f - _wallThickness * 0.5f);

            var wallOffsetY = _floorThickness * 0.5f + _mazeSize.y * .5f;
            _northWall.localPosition = new Vector3(0, wallOffsetY, wallBounds.y);
            _northWall.localScale = new Vector3(_wallThickness, _mazeSize.y, _mazeSize.x - 1);

            _southWall.localPosition = new Vector3(0, wallOffsetY, -wallBounds.y);
            _southWall.localScale = new Vector3(_wallThickness, _mazeSize.y, _mazeSize.x - 1);

            _eastWall.localPosition = new Vector3(wallBounds.x, wallOffsetY, 0);
            _eastWall.localScale = new Vector3(_mazeSize.z, _mazeSize.y, _wallThickness);

            _westWall.localPosition = new Vector3(-wallBounds.x, wallOffsetY, 0);
            _westWall.localScale = new Vector3(_mazeSize.z, _mazeSize.y, _wallThickness);

            _glass.localScale = new Vector3(_mazeSize.x, _glassThickness, _mazeSize.z);
            _floor.localScale = new Vector3(_mazeSize.x, _floorThickness, _mazeSize.z);

            _nePin.localPosition = new Vector3(wallBounds.x - _pinPadding.x, 0, wallBounds.y - _pinPadding.y);
            _swPin.localPosition = new Vector3(-wallBounds.x + _pinPadding.x, 0, -wallBounds.y + _pinPadding.y);
            _nwPin.localPosition = new Vector3(-wallBounds.x + _pinPadding.x, 0, wallBounds.y - _pinPadding.y);
            _sePin.localPosition = new Vector3(wallBounds.x - _pinPadding.x, 0, -wallBounds.y + _pinPadding.y);
        }
    }
}