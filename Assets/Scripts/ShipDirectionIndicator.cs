using UnityEngine;
using UnityEngine.UIElements;

public class ShipDirectionIndicator : MonoBehaviour
{
    [Header("References")]
    public ShipController shipController;

    [Header("Components")]
    public Animator rightIndicatorAnim;
    public Animator leftIndicatorAnim;

    private void Awake()
    {
        shipController = GetComponentInParent<ShipController>();
        if (shipController == null)
        {
            Debug.LogWarning($"{gameObject.name}: is not attached to a shipController parent object.");
            Destroy(gameObject);
            return;
        }
    }

    private void Update()
    {
        IndicatorUpdate();
    }

    #region Indicator

    private void IndicatorUpdate()
    {
        if (rightIndicatorAnim == null || leftIndicatorAnim == null)
        {
            Debug.LogWarning($"{gameObject.name}: One of the indicators are null.");
            return;
        }

        float xInput = shipController.XInput;

        rightIndicatorAnim.SetBool("Active", xInput < 0);
        leftIndicatorAnim.SetBool("Active", xInput > 0);
    }

    #endregion indicator

}
