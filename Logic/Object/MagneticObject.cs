using System;
using System.Collections;
using System.Diagnostics;
using Custom.Logic.Audio;
using Custom.Logic.Enemy;
using Custom.Logic.Object;
using Custom.Logic.Upgrades;
using Engine;
using Engine.DI;
using Engine.Senser;
using Main;
using Template.CharSystem;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;
[RequireComponent(typeof(ObjectParticlesControl))]
public class MagneticObject : MonoBehaviour,ILevelCompleted, ILevelFailed
{
    public bool CanBeMoved=true;
    public bool CanBeDraged=true;
    private float _speedRotation=2;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _gravityHolder;
    [SerializeField] private Damage _damage;
    private float maxOffsetx =3;
    private float maxOffsety =2;
    private float _pushForce=30f;
    private float _angel =60f;
    public bool InList;
    private float _radius;
    private bool _rotating;
    private bool _moving;
    [SerializeField]private Collider _collider;
    float t = 0f;
    bool nonInPos = false;
    float t2 = 0;
    private int _y;
    private float offset;
    Vector3 m_EulerAngleVelocity = new Vector3(0, 100, 0);
    private float _rotationSpeedCorrecor;
    [SerializeField] private ParticleSystem _holdPartocals;
    private Vector3 _lastPosition;
    private Quaternion _lastRotation;
    private ObjectParticlesControl _objectParticlesControl;

    private ILevelBufsControll _levelBufsControll;
    private const string path = "Audio/Hit/mixkit-air-whistle-punch-2048";
    

    private bool _poison;
    private bool _fire;
    private bool _freeze;

    private void Awake()
    {
        _objectParticlesControl = GetComponent<ObjectParticlesControl>();
        _lastPosition = transform.position;
        _lastRotation = transform.rotation;
    }

    private void Start()
    {
        _levelBufsControll = DIContainer.GetAsSingle<ILevelBufsControll>();
    }

    public void ResetAll()
    {
        if(gameObject.activeSelf)
            return;
        InList = false;
        CanBeMoved = true;
        CanBeDraged = true;
        _rigidbody.isKinematic = true;
        transform.position = _lastPosition;
        transform.rotation = _lastRotation;
        _collider.enabled = true;
    }

    public void OnEnable()
    {
        CanBeMoved = true;
        CanBeDraged = true;
        m_EulerAngleVelocity =
            new Vector3(Random.Range(-100f, 100f), Random.Range(-100f, 100f), Random.Range(-100f, 100f));
        _rotationSpeedCorrecor = Random.Range(1f, 1f);
        _rigidbody = GetComponent<Rigidbody>();
        LevelStatueFailed.Subscribe(this);
        LevelStatueCompleted.Subscribe(this);
    }
    public void OnDisable()
    {
        LevelStatueFailed.Unsubscribe(this); 
        LevelStatueCompleted.Unsubscribe(this);
    }

    public void StartRotate(Transform gravityHolderTransgrom,float radius)
    {
        _radius = radius;
        _gravityHolder = gravityHolderTransgrom;
        Rotate();
    }
   

    public void Rotating(Transform gravityHolderTransgrom,float radius)
    {
        _fire = _levelBufsControll.GetBuffLevel(BuffId.Fire) > 1;
        _freeze = _levelBufsControll.GetBuffLevel(BuffId.Freeze) > 1;
        _poison = _levelBufsControll.GetBuffLevel(BuffId.Poison) > 1;
        
        CanBeMoved = false;
        CanBeDraged = false;
        _collider.enabled = false;
        _gravityHolder = gravityHolderTransgrom;
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
        _radius = radius;
        Rotate();
        _objectParticlesControl.Hold();
        if (_fire)
            _objectParticlesControl.Fired();
        if (_poison)
            _objectParticlesControl.Posioned();
        if (_freeze)
            _objectParticlesControl.Freezed();
    }
    

    public void Stop()
    {
        
        InList = false;
        _moving = false;
        _rotating = false;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        if(!gameObject.activeSelf)
            return;
        StartCoroutine(GrabDelay());
        CanBeMoved = true;
        _collider.enabled = true;
        if(_holdPartocals!=null)
            _holdPartocals.Stop();
    }
    public void StopMove(float radius)
    {
        
        _radius = radius;
        Move();
        StartRotate(_gravityHolder,_radius);
        CanBeDraged = false;

    }

    private void FixedUpdate()
    {

            if (CanBeDraged&&_moving)
            {
                t += 0.04f;
                Vector3 direction = (_gravityHolder.position -transform.position ).normalized;

                _rigidbody.MovePosition(transform.position+(direction*(_speedRotation*10+t)*Time.fixedDeltaTime));
                if(t>=4)
                    Stop();
            }
            
            if (!CanBeMoved && _rotating)
            {
                t2 += _speedRotation*_rotationSpeedCorrecor * Time.fixedDeltaTime;
                //float x = Mathf.Cos(t2) * (_radius + offset);
                float x = Mathf.Cos(t2) * (_radius);
                float z = Mathf.Sin(t2) * (_radius);
                float y;
                switch (_y)
                {
                    case 0:
                        y = 1;
                        break;
                    case 1:
                        y = Mathf.Sin(t2)+1;
                        break;
                    case 2:
                        y = Mathf.Sin(-t2)+1;
                        break;
                    case 3:
                        y = Mathf.Cos(t2)+1;
                        break;
                    default:
                        y = Mathf.Cos(-t2)+1;
                        break;
                }
                Vector3 pos = new Vector3(x, y, z);
                Vector3 newPos = ((pos + _gravityHolder.position) - transform.position).normalized;
                if (Vector3.Distance(transform.position, (pos + _gravityHolder.position)) > 0.3f && !nonInPos)
                {
                    _rigidbody.MovePosition(
                        transform.position + (newPos * ((_speedRotation * 8) * Time.fixedDeltaTime)));

                }
                else
                {
                    nonInPos = true;
                    _rigidbody.MovePosition(pos + _gravityHolder.position);
                }
                transform.up=Vector3.up;
               // Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity *3 * Time.fixedDeltaTime);
               // _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
                

            }

            
            if (!_fire)
                _objectParticlesControl.UnFired();
            if (!_poison)
                _objectParticlesControl.UnPosioned();
            if (!_freeze)
                _objectParticlesControl.UnFreezed();
    }

    public void PushInPoint(Transform point)
    {
        _objectParticlesControl.UnHold();
        _moving = false;
        _rotating = false;
        _collider.enabled = true;
        CanBeMoved = true;
        if(!gameObject.activeSelf)
            return;
        StartCoroutine(GrabDelay());
        StopMove(_radius);
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        var mainDirection = ((point.position+new Vector3(0,1,0)) - transform.position ).normalized;
        _rigidbody.AddForce(mainDirection*_pushForce,ForceMode.Impulse);
        transform.up = mainDirection;
    }
    public void PushInConus(Transform playerTransform)
    {
        _objectParticlesControl.UnHold();
        _moving = false;
        _rotating = false;
        _collider.enabled = true;
        CanBeMoved = true;
        StartCoroutine(GrabDelay());
        StopMove(_radius);
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        
        var rot = Quaternion.AngleAxis(_angel, playerTransform.position-_gravityHolder.position);
        var direction = (rot * (playerTransform.position-_gravityHolder.position)).normalized;
        var mainDirection = direction + (_gravityHolder.position - transform.position).normalized;

        _rigidbody.AddForce(mainDirection*_pushForce,ForceMode.Impulse);
    }
    public void PushInRadius()
    {
        _objectParticlesControl.UnHold();
        _moving = false;
        _rotating = false;
        _collider.enabled = true;
        CanBeMoved = true;
        StartCoroutine(GrabDelay());
        StopMove(_radius);
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        _rigidbody.AddForce((transform.position-_gravityHolder.position).normalized*_pushForce,ForceMode.Impulse);

    }
    protected void OnCollisionEnter(Collision collision)
    {
        
        
        if(_rigidbody.velocity.sqrMagnitude<_pushForce)
            return;
        if (collision.gameObject.layer == 6)
        {
            Fighter attacked = collision.transform.GetComponent<Fighter>();
            EnemyDebuffControll enemyDebuffControll = collision.transform.GetComponent<EnemyDebuffControll>();
            if (_poison) 
                enemyDebuffControll.AddDebuff(BuffId.Poison);
            if (_fire) 
                enemyDebuffControll.AddDebuff(BuffId.Fire);
            if (_freeze) 
                enemyDebuffControll.AddDebuff(BuffId.Freeze);
            _damage.SetFighter(attacked);
            attacked.TakeDamage(_damage);
            ISound soundSenserInfo = DIContainer.GetAsSingle<ISound>();
            if (soundSenserInfo.isEnable)
            {
                /*GameObject obj = new GameObject();
                AudioPlay audioPlay= obj.AddComponent<AudioPlay>();
                audioPlay.PlayClip(path,transform.position);*/
            }
            
            gameObject.SetActive(false);
            

        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 9 && other.gameObject.layer != 6 && other.gameObject.layer != 10) return;
        if (!_moving && !_rotating) return;
        InList = false;
        
        Stop();
    }


    private IEnumerator GrabDelay()
    {
        yield return new WaitForSeconds(1f);
        CanBeDraged = true;
        InList = false;
        _fire = false;
        _freeze = false;
        _poison = false;
    }



    private void Rotate()
    {
        _y = Random.Range(0, 3);

        nonInPos = false;
        t2 = 0;
        
        offset = Random.Range(0f, maxOffsetx);
        t2 =  Random.Range(0f, 10f);;
        _moving = false;
        _rotating = true;
    }

    private void Move()
    {
        t = 0;
        _rotating = false;
        _moving = true;
    }
    

    public void LevelCompleted()
    {
        if(CanBeDraged && CanBeMoved)
            return;
        StopAllCoroutines();
        PushInRadius();
    }

    public void SetDamage(Damage damage)
    {
        _damage = damage;
    }

    public void LevelFailed()
    {
        if(CanBeDraged && CanBeMoved)
            return;
        StopAllCoroutines();
        PushInRadius();
    }
    
}