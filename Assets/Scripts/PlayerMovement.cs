using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    private Vector3 inputDir;

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
        if (inputDir.sqrMagnitude < 0.0001f) return;

        // Camera-based directions
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        Vector3 moveDir = camForward * inputDir.z + camRight * inputDir.x;
        moveDir.Normalize();

        Vector3 targetPos = rb.position + moveDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPos);

        Quaternion targetRot = Quaternion.LookRotation(moveDir);
        rb.MoveRotation(targetRot);
    }
}
