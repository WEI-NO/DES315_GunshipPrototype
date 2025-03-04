using UnityEngine;

public class JerkBased2DMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

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

    // Limits
    [SerializeField] private float _AngularMaxSpeed; public float AngularMaxSpeed { get { return _AngularMaxSpeed; } private set { } }
    [SerializeField] private float _AngularMaxAcceleration; public float AngularMaxAcceleration { get { return _AngularMaxAcceleration; } private set { } }
    // Friction
    [SerializeField] private float _AngularFriction; public float AngularFriction { get { return _AngularFriction; } private set { } }

    [SerializeField] private float _AngularEngineDamping; public float AngularEngineDamping { get { return _AngularEngineDamping; } private set { } }

    // Jerk
    [SerializeField] private float _AngularJerk; public float AngularJerk { get { return _AngularJerk; } private set { } }
    // Acceleration
    [SerializeField] private float _AngularAcceleration; public float AngularAcceleration { get { return _AngularAcceleration; } private set { } }
    // Velocity
    [SerializeField] private float _AngularVelocity; public float AngularVelocity { get { return _AngularVelocity; } private set { } }

    #endregion angular

    [Header("Controls")]
    public float YInput = 0;
    public float XInput = 0;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {

    }

    void Update()
    {
        DetectInput_WS();
        DetectInput_AD();
    }

    private void FixedUpdate()
    {
        Movement_FrontBack();
        Movement_Rotation();
    }

    #region Input 
    private void DetectInput_WS()
    {
        YInput = 0;
        if (Input.GetKey(KeyCode.W))
        {
            YInput += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            YInput -= 1;
        }
    }

    private void DetectInput_AD()
    {
        XInput = 0;
        if (Input.GetKey(KeyCode.A))
        {
            XInput += 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            XInput -= 1;
        }
    }
    #endregion input

    #region physics

    private void Movement_FrontBack()
    {
        Vector2 faceDirection = transform.up;

        // Jerk
        Vector2 jerkValue = Time.fixedDeltaTime * _Jerk * YInput;

        // Acceleration
        _Acceleration += jerkValue;
        _Acceleration -= _Acceleration * _EngineDamping * Time.fixedDeltaTime;
        _Acceleration = Vector2.ClampMagnitude(_Acceleration, _MaxAcceleration);

        Vector2 accelerationValue = _Acceleration;
        // Velocity
        _Velocity += accelerationValue;
        _Velocity -= _Velocity * _Friction * Time.fixedDeltaTime;
        _Velocity = Vector2.ClampMagnitude(_Velocity, _MaxSpeed);

        rb.linearVelocity = _Velocity * faceDirection;
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
}
