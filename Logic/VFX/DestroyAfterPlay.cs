using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace Custom.Logic.VFX
{
    public class DestroyAfterPlay : MonoBehaviour
    {
        private VisualEffect _visualEffect;
        [SerializeField] private float _time=3;
        private void Awake()
        {
            _visualEffect = GetComponent<VisualEffect>();
        }

        private void Start()
        {
            WaitAndDestroy();
        }

        private IEnumerator WaitAndDestroy()
        {
            yield return new WaitForSeconds(_time);
            Destroy(gameObject);
        }
    }
}
