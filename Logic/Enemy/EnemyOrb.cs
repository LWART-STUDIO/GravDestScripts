using System;
using Template.CharSystem;
using UnityEngine;

namespace Custom.Logic.Enemy
{
    public class EnemyOrb : MonoBehaviour
    {
        private Damage _damage;
        private float _speed=4;
        private Vector3 _direction;
        public void Init(Damage damage,Vector3 direction)
        {
            _damage = damage;
            _direction = direction;
        }
        private void OnTriggerEnter(Collider collision)
        {
            if(collision.gameObject.layer == 9)
            {
                Destroy(gameObject);
            }
            if (collision.gameObject.TryGetComponent(out Player.Player player))
            {
                player.TakeDamage(_damage);
                Destroy(gameObject);
            }
            
            
            
            
        }

        private void FixedUpdate()
        {
            transform.Translate(_direction*Time.fixedDeltaTime*_speed,Space.World);
        }
    }
}
