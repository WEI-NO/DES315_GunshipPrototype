using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;

public enum Status
{
    ShipSwitch
}

public class ShipStatusHUD : MonoBehaviour
{

    [Header("Properties")]
    public Sprite[] statusSprites;
    public StatusIndicator indicator;
    public Transform statusContainer;

    private void Awake()
    {
        ShipCarrier.OnShipChange += OnShipChange;
    }

    private void OnShipChange(int index, ShipController ship)
    {
        AddStatus(Status.ShipSwitch, ShipCarrier.Instance.shipSwitchCooldown);
    }

    public void AddStatus(Status status, float duration)
    {
        int index = (int)status;

        if (statusSprites == null || index >= statusSprites.Length)
        {
            Debug.LogWarning($"{gameObject.name}: Invalid status sprite - {Status.ShipSwitch}");
            return;
        }

        var newStatus = Instantiate(indicator, statusContainer);
        newStatus.Initialize(statusSprites[index], duration);
    }

}
