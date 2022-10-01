using System.Collections;
using Custom.Extensions;
using Custom.Logic.Coins;
using Engine;
using Engine.DI;
using example1;
using Main;
using Main.Level;
using Template.CharSystem;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


namespace Custom.Logic.Enemy
{
    [RequireComponent(typeof(NavMeshAgent),typeof(EnemyDebuffControll))]
    public class EnemyAI :Fighter,ILevelStarted,ICoroutineRunner
    {
        public float SightRange, AttackRange;
        public float WalkPointRange;
        public float TimeBetweenAttacks;
        public bool UseRandomPoint=true;
        public bool UsePatrolling = true;
        
        [SerializeField] private Attacked _Attacked;
        [SerializeField] private Damage _Damage;
        [SerializeField] private EnemySettings _settings;
        [SerializeField] private GameObject _hitText;


        [SerializeField] private EnemyAnimationControl _animationControl;
        [SerializeField] private LayerMask _groundLayerMask, _playerLayerMask;
        [SerializeField] private Transform[] _path;
        [SerializeField] private EnemyAttackControl _enemyAttackControl;


        private NavMeshAgent _agent;
        private Transform _playerTransform;
        private Vector3 _walkPoint;
        private bool _walkPointSet;
        private bool _playerInSightRange;
        private bool _playerInAttackRange;
        private bool _work;
        private bool _canWakeUp = false;
        
        
        private EnemyInformation _enemyInformation;
        private CoinsDropper _coinsDropper;
        [SerializeField] private GameObject _model;
        public float FillAmount;
        private EnemyInformator _enemyInformator;
        private EnemyDebuffControll _enemyDebuffControll;
        private Coroutine _waitToDistanateCorutine;


        protected override IAttacked DefineAttacked() => _Attacked;
        protected override IDamage DefineDamage() => _Damage;

        private void Awake()
        {
            _enemyDebuffControll = GetComponent<EnemyDebuffControll>();
            _enemyInformator = GetComponent<EnemyInformator>();
            TimeBetweenAttacks = _settings.TimeBetweenAttacks;
            _model.SetActive(false);
        }

        private void OnEnable()
        {
            LevelStatueStarted.Subscribe(this);
            
        }

        private void OnDisable()
        {
            LevelStatueStarted.Unsubscribe(this);
        }

        private void Start()
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Poof"), transform.position, Quaternion.identity);
            Invoke("WaitAndShow",0.5f);
            _coinsDropper = GetComponent<CoinsDropper>();
            _Attacked = new Attacked(_settings.hitPoint, this);
            _Damage = new Damage(this, _settings.damage);
            _Attacked.maxHit = _Attacked.hitPoint;
            _enemyInformation=DIContainer.GetAsSingle<ILevelsManager>().level.EnemyInformation;
            WakeUp();
            _animationControl.Idle();
            _agent = GetComponent<NavMeshAgent>();
            _playerTransform = DIContainer.GetAsSingle<ILevelsManager>().level.PlayerInformation.PlayerTransform;
            
        }

        private void Update()
        {
            FillAmount = _Attacked.hitPoint/_Attacked.maxHit;
            if(!_work)
                return;
            var position = transform.position;
            _playerInSightRange = Physics.CheckSphere(position, SightRange, _playerLayerMask);
            _playerInAttackRange = Physics.CheckSphere(position, AttackRange, _playerLayerMask);
            
            if (!_playerInSightRange && !_playerInAttackRange&& UsePatrolling)
                Patrolling();
            if(_playerInSightRange && !_playerInAttackRange&& UsePatrolling)
                ChasePlayer();
            if(_playerInAttackRange && _playerInSightRange)
                _enemyAttackControl.AttackPlayer(_playerTransform,TimeBetweenAttacks,_Damage);
        }

        private void Patrolling()
        {
            if (_enemyDebuffControll.Freezed)
            {
                _agent.SetDestination(transform.position);
                _animationControl.Idle();
                return;
            }
                
                
            _animationControl.Run();
            if(!_walkPointSet)
                SearchWalkPoint();
            if (_walkPointSet)
                _agent.SetDestination(_walkPoint);
            
            Vector3 distanceToWalkPoint = transform.position - _walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                if(_waitToDistanateCorutine!=null)
                  StopCoroutine(_waitToDistanateCorutine);
                    
                _walkPointSet = false;
            }
                
        }

        private void SearchWalkPoint()
        {
            if (UseRandomPoint)
            {
                float randomZ = Random.Range(-WalkPointRange, WalkPointRange);
                float randomX = Random.Range(-WalkPointRange, WalkPointRange);

                var position = transform.position;
                _walkPoint = new Vector3(position.x + randomX, position.y,
                    position.z + randomZ);

                if (Physics.Raycast(_walkPoint, -transform.up, 2f, _groundLayerMask))
                {
                    _walkPointSet = true;
                   _waitToDistanateCorutine=StartCoroutine(WaitToDestinat());
                }
                    
            }
            else
            {
                _walkPoint = _path[Random.Range(0, _path.Length)].position;
                _walkPointSet = true;
            }
            
        }

        private void ChasePlayer()
        {
            if (_enemyDebuffControll.Freezed)
            {
                _animationControl.Idle();
                _agent.SetDestination(transform.position);
                return;
            }
            _agent.SetDestination(_playerTransform.position);
            _animationControl.Run();
        }

       

       

        private void OnDrawGizmosSelected()
        {
            GizmoTools.DrawGizmoDisk(transform, AttackRange, Color.red,0.2f);

            GizmoTools.DrawGizmoDisk(transform, SightRange, new Color(1f, 0.92f, 0.02f, 0.31f));

            //GizmoTools.DrawGizmoDisk(transform, WalkPointRange, Color.green);
        }

        public void LevelStarted()
        {
            StartCoroutine(WaitToWakeUp());

        }

        private IEnumerator WaitToDestinat()
        {
            yield return new WaitForSeconds(3f);
            _walkPointSet = false;
            
        }

        [NaughtyAttributes.Button()]
        private void ShowHitText(string text="10")
        {
          GameObject obj= Instantiate(_hitText, transform.position+new Vector3(Random.Range(0,1f),1,Random.Range(0,1f)), Quaternion.identity, transform);
          obj.GetComponent<TextMesh>().text = text;
        }

        private void TakeHit(IDamage damage)
        {
            ShowHitText(""+damage.value);
        }
        protected override void OnDead(IDamage damage)
        {
            _enemyInformator.Killed();
            Instantiate(Resources.Load<GameObject>("Prefabs/VFX/Poof"), transform.position, Quaternion.identity);
            _coinsDropper.DropAmpount();
            _enemyInformation.RemoveEnemyAI(this);
            Destroy(gameObject);
        }

        protected override void OnTakeDamage(IDamage damage) => TakeHit(damage);

        private IEnumerator WaitToWakeUp()
        {
            while (!_canWakeUp)
            {
                yield return null;
            }
            _work = true;
        }
        private void WaitAndShow()
        {
            _model.SetActive(true);
            _canWakeUp = true;
        }

       
    }
}
