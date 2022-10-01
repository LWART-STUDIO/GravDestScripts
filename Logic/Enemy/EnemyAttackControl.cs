using System;
using System.Collections;
using Cinemachine;
using Template.CharSystem;
using UnityEngine;
using UnityEngine.AI;

namespace Custom.Logic.Enemy
{
    [RequireComponent(typeof(EnemyDebuffControll))]
    public class EnemyAttackControl : MonoBehaviour
    {
        public bool Attacked;
        [SerializeField] private GameObject Orb;
        [SerializeField] private EnemyType _enemyType;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private EnemyAnimationControl _animationControl;
        [SerializeField] private NavMeshAgent _agent;
        private EnemyDebuffControll _enemyDebuffControll;
        [SerializeField] private EnemyAttackCheck _enemyAttackCheck;

        private void Awake()
        {
            _enemyDebuffControll = GetComponent<EnemyDebuffControll>();
        }

        public void AttackPlayer(Transform playerTransform,float timeBetweenAttacks,Damage damage)
        {
            if(_enemyDebuffControll.Freezed)
                return;
            _animationControl.Idle();
            _agent.SetDestination(transform.position);
            Vector3 direction = ( playerTransform.position-transform.position).normalized;
            transform.forward=new Vector3(direction.x,0,direction.z);
            
            if (!Attacked)
            {
                // _playerTransform.GetComponent<Fighter>().TakeDamage(_Damage);
                if (_enemyType == EnemyType.MeleeAttacker)
                {
                    MeleeWait(damage);
                }
                if (_enemyType == EnemyType.RangeAttacker)
                {
                    StartCoroutine(AnimationRangeFix(playerTransform,damage));
                }
                _animationControl.Attack();
                Attacked = true;
                Invoke(nameof(ResetAttack),timeBetweenAttacks);
            }
        }

        private void MeleeWait(Damage damage)
        {
            StartCoroutine(AnimationMeele(damage));
        }
        private void ResetAttack() =>
            Attacked = false;
        private IEnumerator AnimationRangeFix(Transform playerTransform,Damage damage)
        {
            yield return new WaitForSeconds(0.4f);
            GameObject orb = Instantiate(Orb, _shootPoint.position+new Vector3(0,1,0) , Quaternion.identity,null);
            Vector3 direction = (playerTransform.position-transform.position).normalized;
            orb.GetComponent<EnemyOrb>().Init(damage,direction);
        }
        private IEnumerator AnimationMeele(Damage damage)
        {
            _enemyAttackCheck.Damage = damage;
            _enemyAttackCheck.CanAttack = true;
            yield return new WaitForSeconds(0.2f);
            _enemyAttackCheck.CanAttack = false;
            
        }
    }
}
