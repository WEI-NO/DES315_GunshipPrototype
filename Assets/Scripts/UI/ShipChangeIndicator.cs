using TMPro;
using UnityEngine;

public class ShipChangeIndicator : BaseHUDElement
{
    public TextMeshProUGUI displayText;

    protected override void OnAwake()
    {
        ShipCarrier.OnShipChange += DisplayShipName;
    }

    private void DisplayShipName(int index, ShipController ship)
    {
        if (ship != null)
        {
            displayText.text = $"{ship.ShipName}";
            anim.SetTrigger("Trigger");
        }
    }
}
