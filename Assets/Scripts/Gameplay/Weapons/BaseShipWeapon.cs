using UnityEngine;

public class BaseShipWeapon : MonoBehaviour
{
    #region Base Class
    protected virtual void OnAwake() { }
    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnDestroyed() { }

    protected virtual void OnKeyDown() { }
    protected virtual void OnKeyHeld() { }
    protected virtual void OnKeyRelease() { }
    public void KeyDownEvent() 
    { 
        OnKeyDown(); 
    }
    public void KeyHeldEvent() 
    {
        holding = true;
        OnKeyHeld(); 
    }
    public void KeyReleaseEvent() 
    {
        holding = false;
        OnKeyRelease(); 
    }

    private void Awake()
    {
        currentROF = defaultROF;
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
        OnUpdate();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    private void OnDestroy()
    {
        OnDestroyed();
    }



    #endregion base class

    [Header("Weapon Properties")]
    public KeyCode triggerKey;
    public BaseProjectile projectile;
    public float defaultROF = 1;
    public float currentROF = 1;
    protected float internal_fireCooldown;
    protected float internal_fireTimer;

    [Header("Input Properties")]
    public bool holding;

    public void CleanUp()
    {
        holding = false;
    }

    protected float ResetFireCooldown()
    {
        internal_fireCooldown = 1.0f / currentROF;
        internal_fireTimer = internal_fireCooldown;
        return internal_fireCooldown;
    }

}
