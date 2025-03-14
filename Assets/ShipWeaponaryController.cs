using UnityEngine;
using System.Collections.Generic;

public class ShipWeaponaryController : MonoBehaviour
{

    [Header("Weapon Properties")]
    public List<BaseShipWeapon> ShipWeapons = new List<BaseShipWeapon>();

    private void Awake()
    {
        AddAttachedWeapons();
    }

    private void Update()
    {
        DetectWeaponInput();
    }

    #region Weaponary

    private void AddAttachedWeapons()
    {
        foreach (Transform ch in transform)
        {
            BaseShipWeapon weapon = ch.GetComponent<BaseShipWeapon>();
            if (weapon == null) continue;

            ShipWeapons.Add(weapon);
        }
    }

    private void DetectWeaponInput()
    {
        foreach (var weapon in ShipWeapons)
        {
            bool triggered = false;

            if (Input.GetKeyDown(weapon.triggerKey))
            {
                weapon.KeyDownEvent();
                triggered = true;
            }

            if (Input.GetKey(weapon.triggerKey))
            {
                weapon.KeyHeldEvent();
                triggered = true;
            }

            if (Input.GetKeyUp(weapon.triggerKey))
            {
                weapon.KeyReleaseEvent();
                triggered = true;
            }

            // Prevent holding from happening when tabbing out.
            if (!triggered)
            {
                weapon.CleanUp();
            }
        }
    }

    #endregion weaponary

}
