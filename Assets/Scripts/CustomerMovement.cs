using UnityEngine;

public class CustomerMovement : MonoBehaviour {
    public Transform target;
    public float speed = 2f;
    public float stopDistance = 0.5f;

    private bool reached;

    void Update() {
        if (reached || target == null) return;

        float dist = Vector3.Distance(transform.position, target.position);

        if (dist > stopDistance) {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            Vector3 dir = (target.position - transform.position);
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f) {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 10f * Time.deltaTime);
            }

        } else {
            reached = true;
            Debug.Log("Customer reached counter, showing order...");

            CustomerOrder order = GetComponent<CustomerOrder>();
            if (order != null) {
                order.ShowOrder();
            } else {
                Debug.LogWarning("No CustomerOrder component found on Customer!");
            }
            
        }

    }
    
}
