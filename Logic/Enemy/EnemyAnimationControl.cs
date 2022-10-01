using System.Collections.Generic;
using UnityEngine;

namespace Custom.Logic.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimationControl : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int IdleParameter = Animator.StringToHash("Idle");
        private static readonly int RunParameter = Animator.StringToHash("Run");
        private static readonly int AttackParameter = Animator.StringToHash("Attack");
        [SerializeField] private List<AnimatorOverrideController> _overrideControllers;
        [SerializeField] private EnemyType _enemyType;
        private void SetUpAnimator()
        {
            _overrideControllers.Add(Resources.Load<AnimatorOverrideController>("AnimationControlers/DefaultAttacker"));
            _overrideControllers.Add(Resources.Load<AnimatorOverrideController>("AnimationControlers/MeleeAttacker"));
            _overrideControllers.Add(Resources.Load<AnimatorOverrideController>("AnimationControlers/RangeAttacker"));
        }
        public void SetDefaultAttacker()
        {
            _animator.runtimeAnimatorController = _overrideControllers[0];
        }
        public void SetMeleeAttacker()
        {
            _animator.runtimeAnimatorController = _overrideControllers[1];
        }

        public void SetRangeAttacker()
        {
            _animator.runtimeAnimatorController = _overrideControllers[2];
        }
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            SetUpAnimator();
            switch (_enemyType)
            {
                case EnemyType.Default:
                    SetDefaultAttacker();
                    break;
                case EnemyType.MeleeAttacker:
                    SetMeleeAttacker();
                    break;
                case EnemyType.RangeAttacker:
                    SetRangeAttacker();
                    break;
                
            }
        }

        public void Run()
        {
            _animator.SetBool(IdleParameter,false);
            _animator.SetBool(RunParameter,true);
        }

        public void Idle()
        {
            _animator.SetBool(IdleParameter,true);
            _animator.SetBool(RunParameter,false);
        }

        public void Attack()
        {
            _animator.SetTrigger(AttackParameter);
        }
    }
}
