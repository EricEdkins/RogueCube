using UnityEngine;

public class CameraFollowOrbit : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget; // player
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -8);
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    private Vector3 velocity = Vector3.zero;
    private float yaw;   // left/right rotation
    private float pitch; // up/down rotation

    void Start()
    {
        // Initialize yaw/pitch from current transform
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void LateUpdate()
    {
        if (cameraTarget == null)
            return;

        // --- Mouse orbit when holding right click ---
        if (Input.GetMouseButton(1))
        {
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        // Calculate rotation and target position
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPosition = cameraTarget.position + rotation * offset;

        // Smooth follow
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
        transform.LookAt(cameraTarget);
    }
}
