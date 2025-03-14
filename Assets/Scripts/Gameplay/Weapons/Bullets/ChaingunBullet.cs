using UnityEngine;

public class ChaingunBullet : BaseProjectile
{
    protected override void OnStart()
    {
        YInput = 1.0f;
    }
}
