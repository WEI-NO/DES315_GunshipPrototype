using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("Camera Properties")]
    public float cameraFollowSpeed;
    public Transform followTarget;
    public Vector2 VelocityBased_ZoomRange = new Vector2(5f, 2f);
    public float zoomSpeed;

    [Header("Private Properties")]
    // Zoom
    private float targetZoom;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        } else
        {
            Destroy(gameObject);
            return;
        }

        targetZoom = Camera.main.orthographicSize;

        // == Subscription ==
        ShipCarrier.OnShipChange += (index, ship) => { 
            if (ship == null) { return; } 
            SwitchTarget(ship.transform); 
        };
    }

    private void FixedUpdate()
    {
        CameraFollowTarget();
        CameraZoom();
    }

    #region Camera Movement

    private void CameraFollowTarget()
    {
        if (!followTarget) return;

        Vector2 newPosition = Vector2.Lerp(transform.position, followTarget.position, cameraFollowSpeed * Time.fixedDeltaTime);

        Vector3 clampedPosition = ClampCameraPosition(newPosition);
        transform.position = clampedPosition;
    }

    private void CameraZoom()
    {
        targetZoom = VelocityBased_ZoomRange.x;

        if (followTarget)
        {
            ShipController ship = followTarget.GetComponent<ShipController>();

            if (ship) // Has ship
            {
                float velocityProgress = ship.VelocityProgress;

                targetZoom = Mathf.Lerp(VelocityBased_ZoomRange.x, VelocityBased_ZoomRange.y, velocityProgress);

            } // Not a ship target (Allowed)

        } // No current target (Allowed)



        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetZoom, zoomSpeed * Time.fixedDeltaTime);
    }

    private Vector3 ClampCameraPosition(Vector3 newPosition)
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        Vector2Int halfSize = MapGenerator.Instance.mapDimension / 2;

        float minX = -halfSize.x + cameraWidth;
        float maxX = halfSize.x - cameraWidth;
        float minY = -halfSize.y + cameraHeight;
        float maxY = halfSize.y - cameraHeight;

        // Clamp camera position
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        return new Vector3(newPosition.x, newPosition.y, -10f);
    }

    #endregion camera movement

    #region Target

    public void SwitchTarget(Transform transform)
    {
        followTarget = transform;
    }

    #endregion target

}
