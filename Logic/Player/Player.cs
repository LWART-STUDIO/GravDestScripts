using Custom.Levels;
using Engine;
using Engine.DI;
using example1;
using Template.CharSystem;
using UnityEngine;

namespace Custom.Logic.Player
{
    public class Player : Fighter,ICoroutineRunner
    {
        [Header("Settings")]
        [SerializeField] private PlayerSettings m_Settings;
        [SerializeField] protected Animator m_Animator;

        [SerializeField] private DirectionController m_Movement;
        public Attacked Attack;
        public Damage Damage;

        public PlayerSettings settings => m_Settings;
        public float FillAmount;
        private LevelControl _levelControl;


        protected override IAttacked DefineAttacked()
        {
            Attack= new Attacked(m_Settings.hitPoint, this);
            Attack.maxHit = Attack.hitPoint;
            return Attack;
        }

        protected override IDamage DefineDamage()
        {
            Damage= new Damage(this, m_Settings.damage);
            return Damage;
        }

        public void Awake()
        {
            m_Movement.Teleport();
            WakeUp();
            m_Movement.AllowLook();
            m_Movement.Reset(transform);
        }

        protected void FixedUpdate()
        {
            m_Movement.SetMovingSpeed(Mathf.Clamp01(joystick.ControllerJoystick.vector.magnitude) * m_Settings.movingSpeed);
            m_Movement.SetDirection(joystick.ControllerJoystick.vector * Time.fixedDeltaTime);

            m_Movement.Move(Time.fixedDeltaTime);
            m_Movement.Look(Time.fixedDeltaTime);

        }

        protected void LateUpdate()
        {
            FillAmount = Attack.hitPoint/Attack.maxHit;
            m_Animator.SetInteger("Run", (m_Movement.isMoving) ? 1 : -1);
            // m_Animator.speed = joystick.ControllerJoystick.vector.magnitude;
            m_Animator.SetFloat("Speed",joystick.ControllerJoystick.vector.magnitude);
        }

        protected void OnValidate()
        {
            if (m_Movement == null)
                m_Movement = new DirectionController(GetComponent<CharacterController>(), m_Settings.movingSpeed, m_Settings.lookingSpeed);
        }

        /*protected void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == 6)
            {
                Character attacked = collision.gameObject.GetComponent<Character>();

                if (attacked != null) Attack(attacked);
                //Kill(attacked);
                //attacked.TakeDamage(m_Damage);
                //attacked.Killed(m_Damage);
            }
        }*/

        protected override void OnDead(IDamage damage)
        {
            _levelControl = FindObjectOfType<LevelControl>();
            int progress = ((_levelControl.CurrentWave + 1)/ _levelControl.Waves.Count)*100;
            DIContainer.GetAsSingle<IMakeFailed>().MakeFailed(progress);
            Destroy(gameObject);
        }

        protected override void OnKill(Character victim)
        {
            m_Attacked.hitPoint *= 2;
            m_Damage.value *= 2;

            Debug.Log("Damage: " + m_Damage.value + ", HitPoint: " + m_Attacked.hitPoint);
        }
    }
}
