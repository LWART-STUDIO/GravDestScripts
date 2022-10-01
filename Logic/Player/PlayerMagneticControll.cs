using System;
using System.Collections.Generic;
using Custom.Extensions;
using Custom.Extention;
using Custom.Levels;
using Custom.Logic.Enemy;
using Custom.Logic.Upgrades;
using Engine;
using Engine.DI;
using Engine.Input;
using Main;
using Main.Level;
using UnityEngine;

namespace Custom.Logic
{
    public class PlayerMagneticControll : MonoBehaviour,IDrag,IEndDrag,IBeginDrag,ILevelStarted
    {
        [SerializeField] private float _sphereMainRadius;
        [SerializeField] private float _sphereStartRadius;
        [SerializeField] private float _defaultSphereMainRadius;
        [SerializeField] private float _defaultSphereStartRadius;
        [SerializeField] private Transform _centerPoint;
        [SerializeField] private GameObject _gravityHolder;
        private List<MagneticObject> _magneticObjects=new List<MagneticObject>();
        public List<MagneticObject> MagneticObjects => _magneticObjects;
        private BuffId _amountBuffId = BuffId.AmountOfObjects;
        private LevelBuff _amountLevelBuff;
        [SerializeField] private Animator _animator;
        private bool _blokGravity = true;
        private bool _isDraggingState = false;
        [SerializeField] private int _maxCount=30;
        [SerializeField] private int _defaultMaxCont = 30;
        [SerializeField] private ParticleSystem _getParticleSystem;
        [SerializeField] private float _atackRange;
        public int MaxCount => _maxCount;
        private KdTree<EnemyAI> _enemyAi;
        private EnemyAI _nearestEnemy;
        private bool _canPush;
        [SerializeField] private Player.Player _player;
        private LevelControl _levelControl;


        private void Awake()
        {
            _levelControl = FindObjectOfType<LevelControl>();
        }

        private void OnEnable()
        {
            InputEvents.Drag.Subscribe(this);
            InputEvents.EndDrag.Subscribe(this);
            InputEvents.BeginDrag.Subscribe(this);
            //_levelBuff = DIContainer.GetAsSingle<ILevelBufsControll>().LevelsBuffs.Find(x => x.BuffId == _buffId);
            _amountLevelBuff = DIContainer.GetAsSingle<ILevelBufsControll>().LevelsBuffs.Find(x => x.BuffId == _amountBuffId);
            LevelStatueStarted.Subscribe(this);

        }
        private void OnDisable()
        {
            InputEvents.Drag.Unsubscribe(this);
            InputEvents.EndDrag.Unsubscribe(this);
            InputEvents.BeginDrag.Unsubscribe(this);
            LevelStatueStarted.Unsubscribe(this);
        }



        private void FixedUpdate()
        { 
            if(Time.timeScale!=1)
                return;
            if(_blokGravity)
                return;
            _enemyAi.UpdatePositions();
            var enemyAi=_enemyAi.FindClosest(transform.position);
            if (_nearestEnemy!=null&&(transform.position - _nearestEnemy.transform.position).magnitude > _atackRange)
            {
                _nearestEnemy.GetComponent<EnemySelectMarker>().Diselect();
            }
            if (enemyAi!=null&&(transform.position - enemyAi.transform.position).magnitude <= _atackRange)
            {
                if(_nearestEnemy!=null)
                    if (_nearestEnemy != enemyAi)
                        _nearestEnemy.GetComponent<EnemySelectMarker>().Diselect();

                _nearestEnemy = enemyAi;
                _nearestEnemy.GetComponent<EnemySelectMarker>().Select();
                _canPush = true;
            }
            else
            {
                _canPush = false;
            }
            // _gravityHolder.transform.localPosition = transform.position;
            foreach (MagneticObject magneticObject in _magneticObjects)
            {
                if (!magneticObject.InList)
                {
                    _magneticObjects.Remove(magneticObject);
                    magneticObject.Stop();
                    break;
                }
            }
            if(!_isDraggingState&&_canPush)
                Push();
        }

        private void OnDrawGizmosSelected()
        {
            //Gizmos.DrawSphere(_centerPoint.position, _sphereMainRadius);
            GizmoTools.DrawGizmoDisk(_centerPoint,_sphereMainRadius,Color.red,0.1f);
            //  Gizmos.DrawSphere(_centerPoint.position,_sphereStartRadius);
            GizmoTools.DrawGizmoDisk(_centerPoint,_sphereStartRadius,new Color(1f, 0.92f, 0.02f, 0.48f));
            
        }

        public void NewWave()
        {
            foreach (MagneticObject magneticObject in _magneticObjects)
            {
                magneticObject.PushInRadius();
            }
            _magneticObjects.Clear();
        }

        public void OnDrag(InputInfo data)
        {
            if(Time.timeScale!=1)
                return;
            if(_blokGravity)
                return;

            _sphereMainRadius = _defaultSphereMainRadius;//+(_levelBuff.Level*_levelBuff.MultiPlier)-1;
            _sphereStartRadius = _defaultSphereStartRadius;// +(_levelBuff.Level*_levelBuff.MultiPlier)-1;
            _maxCount = _defaultMaxCont;// +(int)(_amountLevelBuff.Level * _amountLevelBuff.MultiPlier) -(int)_amountLevelBuff.MultiPlier;
            Collider[] hitColiders = Physics.OverlapSphere(_centerPoint.position, _sphereStartRadius);
            foreach (var colider in hitColiders)
            {
                if (colider.TryGetComponent(out MagneticObject magneticObject))
                {
                    if (!magneticObject.InList&&magneticObject.CanBeMoved&&magneticObject.CanBeDraged&& _magneticObjects.Count <= _maxCount)
                    {
                        magneticObject.SetDamage(_player.Damage );
                        magneticObject.InList = true; 
                        magneticObject.Rotating(_gravityHolder.transform,_sphereMainRadius);
                        _magneticObjects.Add(magneticObject);
                        
                    }
                }

            }
          
        }


        private void Push()
        {
            if(_magneticObjects.Count<=0)
                return;
            Vector3 direction = (_nearestEnemy.transform.position - transform.position).normalized;
            transform.forward = new Vector3(direction.x,0,direction.z);
            _animator.SetTrigger("Push");
            foreach (MagneticObject magneticObject in _magneticObjects)
            {
                if (!magneticObject.InList)
                {
                    magneticObject.Stop();
                }
                else
                {
                    magneticObject.PushInPoint(_nearestEnemy.transform);
                }
                
            }
            _magneticObjects.Clear();
        }
        public void OnEndDrag(InputInfo data)
        {
            if(_blokGravity)
                return;
            if(Time.timeScale!=1)
                return;
            _isDraggingState = false;
            if(!_canPush)
                return;
           Push();
            
        }

        public void OnBeginDrag(InputInfo data)
        {

            if(_blokGravity)
                return;
            _levelControl.TryActivateEnemy();
            //_getParticleSystem.Play();
            _animator.SetTrigger("HandsUp");
            _isDraggingState = true;
        }

        public void LevelStarted()
        {
            _enemyAi = DIContainer.GetAsSingle<ILevelsManager>().level.EnemyInformation.EnemyAi;
            _blokGravity = false;
        }

      
 
    }
}
