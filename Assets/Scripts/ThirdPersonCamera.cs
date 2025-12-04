using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Position")]
    [SerializeField] private float distance = 5f;
    [SerializeField] private float height = 2f;

    [Header("Rotation")]
    [SerializeField] private float mouseSensitivity = 3f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 60f;

    private float yaw;
    private float pitch;

    private void Start() {
        if (target == null) {
            Debug.LogError("ThirdPersonCamera: No target assigned!");
            enabled = false;
            return;
        }

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = Mathf.Clamp(angles.x, minPitch, maxPitch);

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void LateUpdate() {
        if (target == null) return;

        // Mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        yaw   += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        transform.position = target.position + offset;

        transform.LookAt(target.position + Vector3.up * height);
    }

}
