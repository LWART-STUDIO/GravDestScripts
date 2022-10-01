using System;
using Template.CharSystem;
using UnityEngine;

namespace Custom.Logic.Enemy
{
    public class EnemyAttackCheck : MonoBehaviour
    {
        public bool CanAttack;
        public Damage Damage;

        private void OnTriggerEnter(Collider other)
        {
            if (CanAttack)
            {
                if (other.TryGetComponent(out Player.Player player))
                {
                    player.TakeDamage(Damage);
                }
            }
        }
    }
}
