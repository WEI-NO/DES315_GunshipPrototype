using System;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEntity : MonoBehaviour
{
    #region Base Class
    protected virtual void OnAwake() { }
    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnDestroyed() { }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        OnHealthChange += HealthChange;
        OnAwake();
    }

    private void Start()
    {
        currentHealth = maxHealth;
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

    public Action<float, bool> OnHealthChange;
    public Action OnHealthZero;

    [Header("Components")]
    protected Animator anim;
    protected Rigidbody2D rb;

    [Header("Entity Health Properties")]
    public bool canHurt;
    public bool useAnimation;
    public float maxHealth;
    protected float currentHealth;

    
    
    #region Health

    public void Hurt(float health)
    {
        if (!canHurt) return;

        if (health <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth - health, 0, maxHealth);
        OnHealthChange?.Invoke(currentHealth, false);
    }

    public void Heal(float health)
    {
        if (!canHurt) return;

        if (health <= 0) return;

        currentHealth = Mathf.Clamp(currentHealth + health, 0, maxHealth);
        OnHealthChange?.Invoke(currentHealth, true);
    }

    public void HealthZero()
    {
        if (!canHurt) return;
        OnHealthZero?.Invoke();
        HealthZeroEvent();
    }

    public void HealthChange(float health, bool positive)
    {
        // Death Check
        if (health <= 0) HealthZero();

        if (useAnimation && anim != null)
        {
            if (positive)
            {
                // Heal
                anim.SetTrigger("Heal");
            }
            else
            {
                // Hurt
                anim.SetTrigger("Hurt");
            }
        }
    }

    protected virtual void HealthZeroEvent()
    {
        print("Not Implemented: Default Death");
        Destroy(gameObject);
    }

    #endregion health

}
