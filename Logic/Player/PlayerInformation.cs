using System;
using UnityEngine;

namespace Custom.Logic
{
    public class PlayerInformation:MonoBehaviour
    {
        [SerializeField] private Transform _cameraPoint;
        [SerializeField] private PlayerMagneticControll _playerMagneticControll;
        [SerializeField] private Player.Player _player;
        public float _currentObjects;
        public float _maxObjects;
        public Transform PlayerTransform;
        public Transform CameraPoint => _cameraPoint;
        public Player.Player Player => _player;
        

        private void Update()
        {
            _currentObjects = _playerMagneticControll.MagneticObjects.Count;
            _maxObjects = _playerMagneticControll.MaxCount;
        }
    }
}