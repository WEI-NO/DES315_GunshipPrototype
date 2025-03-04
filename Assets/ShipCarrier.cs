using UnityEngine;
using System.Collections.Generic;
using System;

public class ShipCarrier : MonoBehaviour
{
    public static ShipCarrier Instance;
    public static Action<int, ShipController> OnShipChange;

    [Header("Static Properties")]
    public static ShipController CurrentShip;
    public static int CurrentShipIndex = -1;

    [Header("Carrier Properties")]
    public List<ShipController> allAssignedShips = new List<ShipController>();

    [Header("Gameplay Properties")]
    public float shipSwitchCooldown;
    private float shipSwitchTimer;

    [Header("Visual Effects")]
    public GameObject onEquipParticleEffect;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // == Subscriptions ==
        OnShipChange += ShipChangeCallback;
    }

    private void Start()
    {
        SwitchToShip(0);
    }

    private void Update()
    {
        shipSwitchTimer -= Time.deltaTime;

        ShipSwitchingLogic();
    }

    #region Ship Carrier
    private void ShipSwitchingLogic()
    {
        int currentInput = DetectNumberKey();

        SwitchToShip(currentInput);
    }

    private void SwitchToShip(int shipIndex)
    {
        // Cooldown 
        if (shipSwitchTimer > 0) return;

        if (shipIndex == -1) return; // No input detected (Allowed)
        // Current Input out of range (Not Allowed)
        if (shipIndex >= allAssignedShips.Count || allAssignedShips == null)
        {
            Debug.LogWarning($"{gameObject.name}: Not a valid input, {shipIndex}");
            return;
        }

        if (CurrentShipIndex == shipIndex) return;

        Vector3 lastLocation = Vector3.zero;
        Quaternion lastRotation = Quaternion.identity;

        if (CurrentShip != null)
        {
            lastLocation = CurrentShip.transform.position;
            lastRotation = CurrentShip.transform.rotation;
            // Clean up logic for the ship
            Destroy(CurrentShip.gameObject);
        }
        ShipController newShip = allAssignedShips[shipIndex];

        if (newShip)
        {
            CurrentShip = Instantiate(newShip, lastLocation, lastRotation);
            CurrentShipIndex = shipIndex;
            OnShipChange?.Invoke(CurrentShipIndex, CurrentShip);
        }
        else
        {
            CurrentShipIndex = -1;
            CurrentShip = null;
        }
    }

    private void ShipChangeCallback(int index, ShipController ship)
    {
        ResetSwitchTimer();
        if (onEquipParticleEffect && ship != null)
        {
            Vector3 location = ship.transform.position;
            var effect = Instantiate(onEquipParticleEffect, location, Quaternion.identity);
        }
    }

    #endregion ship carrier

    #region Gameplay

    private void ResetSwitchTimer()
    {
        shipSwitchTimer = shipSwitchCooldown;
    }

    #endregion gameplay

    #region Input


    private int DetectNumberKey()
    {
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                return i - 1;
            }
        }
        return -1;
    }

    #endregion input

}
