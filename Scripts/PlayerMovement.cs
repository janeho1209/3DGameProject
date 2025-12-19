using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private float turnSpeed = 10f;

    private Rigidbody rb;
    private Vector3 inputDir;

    private Vector3 currentVelocity;

    private void Awake() {
        rb = GetComponent<Rigidbody>();

        if (cameraTransform == null && Camera.main != null) {
            cameraTransform = Camera.main.transform;
        }

    }

    private void Update() {
        // WASD controls
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputDir = new Vector3(x, 0f, z);
        inputDir = Vector3.ClampMagnitude(inputDir, 1f);
    }

    private void FixedUpdate() {

        // Camera-based directions
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        moveDir.Normalize();

        Vector3 targetVelocity = moveDir * moveSpeed;

        float smooth = inputDir.sqrMagnitude > 0.01f ? acceleration : deceleration;

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, smooth * Time.fixedDeltaTime);

        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);

        if (currentVelocity.sqrMagnitude > 0.01f) {
            Quaternion targetRot = Quaternion.LookRotation(currentVelocity.normalized);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime));
        }
        
    }
}
