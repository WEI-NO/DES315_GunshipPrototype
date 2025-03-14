using UnityEngine;
using UnityEngine.Windows;

public class BaseProjectile : MonoBehaviour
{
    #region Base Class
    protected virtual void OnAwake() { }
    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnDestroyed() { }

    public virtual void OnKeyDown() { }
    public virtual void OnKeyHeld() { }
    public virtual void OnKeyRelease() { }

    private void Awake()
    {
        lifeTimer = lifeTime;
        rb = GetComponent<Rigidbody2D>();
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void OnEnable()
    {
        OnEnabled();
    }

    private void OnDisable()
    {
        OnDisabled();
    }

    private void Update()
    {
        LifeTimeUpdate();
        OnUpdate();
    }

    private void FixedUpdate()
    {
        Movement_FrontBack();
        Movement_Rotation();
        OnFixedUpdate();
    }

    private void OnDestroy()
    {
        OnDestroyed();
    }



    #endregion base class

    [Header("Component")]
    public Rigidbody2D rb;

    [Header("Projectile Properties")]
    public bool homing;
    public float lifeTime;
    private float lifeTimer;

    [Header("Damage Properties")]
    public float attackDamage;
    public GameObject hitEffect;

    [Header("Physics Properties - WS")]
    #region Forward and Backward
    // Limits
    [SerializeField] private float _MaxSpeed; public float MaxSpeed { get { return _MaxSpeed; } private set { } }
    [SerializeField] private float _MaxAcceleration; public float MaxAcceleration { get { return _MaxAcceleration; } private set { } }
    // Friction
    [SerializeField] private float _Friction; public float Friction { get { return _Friction; } private set { } }

    [SerializeField] private float _EngineDamping; public float EngineDamping { get { return _EngineDamping; } private set { } }

    // Jerk
    [SerializeField] private Vector2 _Jerk; public Vector2 Jerk { get { return _Jerk; } private set { } }
    // Acceleration
    [SerializeField] private Vector2 _Acceleration; public Vector2 Acceleration { get { return _Acceleration; } private set { } }
    // Velocity
    [SerializeField] private Vector2 _Velocity; public Vector2 Velocity { get { return _Velocity; } private set { } }
    #endregion forward and backward

    [Header("Physics Properties - AD")]
    #region Angular

    [Header("Controls")]
    public float YInput = 0;
    public float XInput = 0;

    [Header("Properties")]
    public float VelocityProgress;

    // Limits
    [SerializeField] protected float _AngularMaxSpeed; public float AngularMaxSpeed { get { return _AngularMaxSpeed; } private set { } }
    [SerializeField] protected float _AngularMaxAcceleration; public float AngularMaxAcceleration { get { return _AngularMaxAcceleration; } private set { } }
    // Friction
    [SerializeField] protected float _AngularFriction; public float AngularFriction { get { return _AngularFriction; } private set { } }

    [SerializeField] protected float _AngularEngineDamping; public float AngularEngineDamping { get { return _AngularEngineDamping; } private set { } }

    // Jerk
    [SerializeField] protected float _AngularJerk; public float AngularJerk { get { return _AngularJerk; } private set { } }
    // Acceleration
    [SerializeField] protected float _AngularAcceleration; public float AngularAcceleration { get { return _AngularAcceleration; } private set { } }
    // Velocity
    [SerializeField] protected float _AngularVelocity; public float AngularVelocity { get { return _AngularVelocity; } private set { } }

    #endregion angular

    #region physics

    private void Movement_FrontBack()
    {
        Vector2 faceDirection = transform.up;

        // Jerk
        Vector2 jerkValue = Time.fixedDeltaTime * _Jerk * YInput * faceDirection;

        // Acceleration
        _Acceleration += jerkValue;
        _Acceleration -= _Acceleration * _EngineDamping * Time.fixedDeltaTime;
        _Acceleration = Vector2.ClampMagnitude(_Acceleration, _MaxAcceleration);

        Vector2 accelerationValue = _Acceleration;
        // Velocity
        _Velocity += accelerationValue;
        _Velocity -= _Velocity * _Friction * Time.fixedDeltaTime;
        _Velocity = Vector2.ClampMagnitude(_Velocity, _MaxSpeed);
        VelocityProgress = _Velocity.magnitude / _MaxSpeed;

        rb.linearVelocity = _Velocity;

    }

    private void Movement_Rotation()
    {
        // Jerk
        float jerkValue = Time.fixedDeltaTime * _AngularJerk * XInput;

        // Acceleration
        _AngularAcceleration += jerkValue;
        _AngularAcceleration -= _AngularAcceleration * _AngularEngineDamping * Time.fixedDeltaTime;
        _AngularAcceleration = Mathf.Clamp(_AngularAcceleration, -_AngularMaxAcceleration, _AngularAcceleration);

        float accelerationValue = _AngularAcceleration;
        // Velocity
        _AngularVelocity += accelerationValue;
        _AngularVelocity -= _AngularVelocity * _Friction * Time.fixedDeltaTime;
        _AngularVelocity = Mathf.Clamp(_AngularVelocity, -_MaxSpeed, _MaxSpeed);

        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + _AngularVelocity);
    }

    #endregion physics


    #region Bullet

    public void SetOrientation(Vector2 faceDirection)
    {
        transform.up = faceDirection;
    }

    private void LifeTimeUpdate()
    {
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            KillBullet();
        }
    }

    public virtual void OnCollisionHit(BaseEntity target)
    {
        target.Hurt(attackDamage);
        KillBullet();
    }

    public void SpawnHitEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }

    public void KillBullet(bool force = false)
    {
        if (!force)
        {
            SpawnHitEffect();
        }
        Destroy(gameObject);
    }

    #endregion bullet

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<BaseEntity>();
        if (entity != null)
        {
            OnCollisionHit(entity);
        }
    }
}
