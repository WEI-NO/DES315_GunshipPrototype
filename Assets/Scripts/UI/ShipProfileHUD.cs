using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipProfileHUD : MonoBehaviour
{
    [Header("Components")]
    public TextMeshProUGUI shipNameText;
    public Image shipIcon;

    [Header("Settings")]
    public float iconScale = 2;

    private void Awake()
    {
        ShipCarrier.OnShipChange += ChangeProfile;
    }

    private void ChangeProfile(int index, ShipController ship)
    {
        if (ship == null) return;

        Sprite shipSprite = ship.shipSprite.sprite;
        Vector2 size = new Vector2(shipSprite.rect.width, shipSprite.rect.height);
        shipIcon.sprite = ship.shipSprite.sprite;
        shipIcon.GetComponent<RectTransform>().sizeDelta = size * iconScale;

        shipNameText.text = $"{ship.ShipName}";
        
    }
}
