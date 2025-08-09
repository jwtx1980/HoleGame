using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The hole to follow (assign in Inspector)
    public Vector3 initialOffset = new Vector3(0, 10, -5); // Default offset when hole scale is 1
    public float zoomFactor = 0.5f; // How much extra zoom per unit growth (e.g., 0.5 means for each extra unit of scale, offset increases by 50%)
    public float smoothSpeed = 2f; // Smoothing effect for camera movement

    private Vector3 currentOffset;

    void Start()
    {
        currentOffset = initialOffset; // Set initial camera distance
    }

    // Call this to reset the camera zoom to its initial state
    public void ResetZoom()
    {
        currentOffset = initialOffset;
        if (target != null)
        {
            transform.position = target.position + currentOffset;
        }
    }

    void LateUpdate()
    {
        if (target != null)
        {
            // Calculate a scaling factor based on the hole's size.
            // Assumes default hole scale is 1. If target.localScale.x > 1, the factor increases.
            float scaleFactor = 1 + (target.localScale.x - 1) * zoomFactor;
            currentOffset = initialOffset * scaleFactor;

            // Smoothly update the camera's position based on the new offset
            Vector3 desiredPosition = target.position + currentOffset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            // Ensure the camera always looks at the hole
            transform.LookAt(target.position);
        }
    }
}
