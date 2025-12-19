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
    [SerializeField] private float mouseSmoothing = 8f;
    [SerializeField] private float swayAmount = 2f;
    [SerializeField] private float swaySmoothness = 6f;

    private Vector2 smoothMouse;
    private Vector2 mouseVelocity;
    private float currentSway;
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
        // NEW:

        Vector2 rawMouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")) * mouseSensitivity;
        smoothMouse = Vector2.SmoothDamp(smoothMouse, rawMouseInput, ref mouseVelocity, 1f / mouseSmoothing);

        // OLD:
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        float targetSway = -smoothMouse.x * swayAmount;
        currentSway = Mathf.Lerp(currentSway, targetSway, Time.deltaTime * swaySmoothness);


        yaw += smoothMouse.x;
        pitch -= smoothMouse.y;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, currentSway);

        Vector3 offset = rotation * new Vector3(0f, height, -distance);
        transform.position = target.position + offset;

        transform.LookAt(target.position + Vector3.up * height);
    }

}
