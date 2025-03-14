using UnityEngine;

public class AsteroidEntity : BaseEntity
{
    [Header("Properties")]
    public float spinSpeed;

    protected override void OnFixedUpdate()
    {
        AutoSpinUpdate();
    }


    #region Visual

    void AutoSpinUpdate()
    {
        float deltaRotation = spinSpeed * Time.fixedDeltaTime;
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(eulerAngles.x, eulerAngles.y, eulerAngles.z + deltaRotation);
    }

    #endregion visual
}
