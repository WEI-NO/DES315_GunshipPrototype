using UnityEngine;

public class ChaingunWeapon : BaseShipWeapon
{
    [Header("Chain Gun Properties")]
    public float rampUpSpeed;
    public float maxShootSpeed;
    public float currentShootSpeed;
    public float shootSpeedMultiplier = 1.0f;
    public float spreadAngle = 30.0f;

    protected override void OnUpdate()
    {
        if (!holding)
        {
            currentShootSpeed -= rampUpSpeed * Time.deltaTime;
            currentShootSpeed = Mathf.Clamp(currentShootSpeed, 0, maxShootSpeed);
        }
    }

    protected override void OnKeyDown()
    {
        
    }

    protected override void OnKeyHeld()
    {
        currentShootSpeed += rampUpSpeed * Time.deltaTime;
        currentShootSpeed = Mathf.Clamp(currentShootSpeed, 0, maxShootSpeed);
        currentROF = defaultROF + currentShootSpeed * shootSpeedMultiplier;
        internal_fireTimer -= Time.deltaTime;
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
