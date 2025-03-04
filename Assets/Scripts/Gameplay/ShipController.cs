using UnityEngine;

public class JerkBased2DMovement : MonoBehaviour
{
    [Header("Components")]
    private Rigidbody2D rb;

    [Header("Physics Properties")]
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

    [Header("Controls")]
    public float YInput = 0;

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
    }

    private void FixedUpdate()
    {
        Movement_FrontBack();
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

    #endregion physics
}
