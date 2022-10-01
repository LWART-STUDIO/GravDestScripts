using System;
using System.Collections;
using Custom.Logic.Upgrades;
using Engine.DI;
using Template.CharSystem;
using UnityEngine;

namespace Custom.Logic.Enemy
{
    public class EnemyDebuffControll : MonoBehaviour
    {
        public bool Poisned => _poisned;
        public bool Fired => _fired;
        public bool Freezed => _freezed;
        [SerializeField] private GameObject _freezeEffect;
        [SerializeField] private GameObject _fireEffect;
        [SerializeField] private GameObject _posionEffect;
        [SerializeField] private GameObject _freezeObject;
        
        
        private EnemyAI _enemyAI;
        private Damage _posionDamage;
        private Damage _fireDamage;

        private bool _poisned;
        private bool _fired;
        private bool _freezed;
        
        private int _poisonCount;
        private int _fireCount;
        private int _freezeCount;
        

        private float _debuffTime=3f;
        private ILevelBufsControll _levelBufsControll;
        
        


        private void Start()
        {
            _enemyAI = GetComponent<EnemyAI>();
            _levelBufsControll = DIContainer.GetAsSingle<ILevelBufsControll>();
        }

        private void Update()
        {
            _fired = _fireCount > 0;
            _freezed = _freezeCount > 0;
            _poisned = _poisonCount > 0;

            _fireEffect.SetActive(_fired);
            _freezeEffect.SetActive(_freezed);
            _posionEffect.SetActive(_poisned);
            _freezeObject.SetActive(_freezed);
        }

        public void AddDebuff(BuffId buffId)
        {
            switch (buffId)
            {
                case (BuffId.Fire):
                    Debug.Log("StartFire");
                    StartCoroutine(StartFire());
                    break;
                case (BuffId.Freeze):
                    StartCoroutine(StartFreeze());
                    break;
                case (BuffId.Poison):
                    StartCoroutine(StartPoison());
                    break;
                default:
                    break;
            }
        }

        public IEnumerator StartFire()
        {
            Damage damage = _levelBufsControll.GetDamageFromBuff(BuffId.Fire);
            float time = _levelBufsControll.DebuffTime(BuffId.Fire);
            Coroutine corutine = StartCoroutine(DoFirening(damage,time));
            _fireCount++;
            yield return new WaitForSeconds(_debuffTime);
            StopCoroutine(corutine);
            _fireCount--;

        }

        public IEnumerator StartFreeze()
        {
            float time = _levelBufsControll.DebuffTime(BuffId.Freeze);
            Coroutine corutine = StartCoroutine(DoFreezing(time));
            _freezeCount++;
            yield return new WaitForSeconds(_debuffTime);
            StopCoroutine(corutine);
            _freezeCount--;

        }

        public IEnumerator StartPoison()
        {
            Damage damage = _levelBufsControll.GetDamageFromBuff(BuffId.Poison);
            float time = _levelBufsControll.DebuffTime(BuffId.Fire);
            Coroutine corutine = StartCoroutine(DoPoisining(damage,time));
            _poisonCount++;
            yield return new WaitForSeconds(_debuffTime);
            StopCoroutine(corutine);
            _poisonCount--;

        }

        public IEnumerator DoFreezing(float tick)
        {
            while (true)
            {
                yield return new WaitForSeconds(tick);
            }
            
        }

        public IEnumerator DoPoisining(Damage damage,float tick)
        {
   
            while (true)
            {
                _enemyAI.TakeDamage(damage);
                yield return new WaitForSeconds(tick);
            }
            
        }

        public IEnumerator DoFirening(Damage damage,float tick)
        {
            while (true)
            {
                _enemyAI.TakeDamage(damage);
                yield return new WaitForSeconds(tick);
            }
            
        }
    }
}
