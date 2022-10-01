using System;
using UnityEngine;

namespace Custom.Logic.VFX
{
    public class LookAtCamera : MonoBehaviour
    {
        private GameObject _camera;

        private void Awake()
        {
            _camera = Camera.main.gameObject;
        }

        private void Update()
        {
            transform.LookAt(_camera.transform.position);
        }
    }
}
