using UnityEngine;

public class MissileLauncherWeapon : BaseShipWeapon
{
    [Header("Chain Gun Properties")]
    public float spreadAngle = 30.0f;

    protected override void OnUpdate()
    {
        internal_fireTimer -= Time.deltaTime;
    }

    protected override void OnKeyDown()
    {
        
    }

    protected override void OnKeyHeld()
    {
        if (internal_fireTimer <= 0)
        {
            Shoot();
            ResetFireCooldown();
        }
    }

    protected override void OnKeyRelease()
    {
        
    }

    private void Shoot()
    {
        if (projectile == null) return;

        BaseProjectile newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        Vector2 spreadDirection = Quaternion.Euler(0, 0, Random.Range(-spreadAngle, spreadAngle)) * transform.up;
        newProjectile.SetOrientation(spreadDirection);
    }
}
