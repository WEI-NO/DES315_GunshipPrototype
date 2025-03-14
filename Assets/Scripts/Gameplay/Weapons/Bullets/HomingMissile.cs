using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HomingMissile : BaseProjectile
{
    protected override void OnFixedUpdate()
    {
        print(CalculateStoppingDistance(AngularVelocity, AngularAcceleration, AngularJerk, AngularMaxAcceleration, Time.fixedDeltaTime));
    }

    private float CalculateStoppingDistance(float velocity, float acceleration, float jerk, float maxAcceleration, float dt)
    {
        float distance = 0.0f;
        while (velocity > 0.0f)
        {
            if (acceleration < maxAcceleration)
            {
                acceleration += (dt * jerk) - acceleration * _AngularEngineDamping * Time.fixedDeltaTime;
            }

            if (acceleration > maxAcceleration) acceleration = maxAcceleration;
            if (velocity > 0.0f) velocity -= dt * acceleration;
            if (velocity < 0.0f) velocity = 0.0f;
            distance += dt * velocity;
        }
        return distance;
    }
}
