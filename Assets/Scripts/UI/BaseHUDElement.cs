using UnityEngine;

public abstract class BaseHUDElement : MonoBehaviour
{
    private const float InterruptThreshold = 0.1f;

    protected virtual void OnAwake() { }
    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void OnDestroyed() { }

    [Header("Components")]
    public Animator anim;

    [Header("Main UI")]
    public bool activeState;

    [Header("Controls")]
    public KeyCode toggleKeycode = KeyCode.None;
    public bool playerInputInterrupt;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        SetMainUIState(false, true);
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
        if (toggleKeycode != KeyCode.None)
        {
            if (Input.GetKeyDown(toggleKeycode))
            {
                ToggleMainUIState();
            }
        }

        if (playerInputInterrupt)
        {
            if (ShipCarrier.CurrentShip != null)
            {
                if (Mathf.Abs(ShipCarrier.CurrentShip.XInput) >= InterruptThreshold
                    || Mathf.Abs(ShipCarrier.CurrentShip.YInput) >= InterruptThreshold)
                {
                    SetMainUIState(false);
                }
            }
        }

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

    #region Animation

    protected void PlayAnimation(string triggerName)
    {
        if (anim == null) return;
        anim.SetTrigger(triggerName);
    }

    #endregion animation

    #region Main UI

    public void ToggleMainUIState()
    {
        activeState = !activeState;

        if (activeState)
        {
            PlayAnimation("Open");
        }
        else
        {
            PlayAnimation("Close");
        }

    }

    public void SetMainUIState(bool state, bool force = false)
    {
        if (activeState == state && !force) return;

        activeState = !state;
        ToggleMainUIState();
    }

    #endregion main ui

}
