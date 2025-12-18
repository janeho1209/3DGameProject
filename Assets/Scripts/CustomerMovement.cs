using UnityEngine;

public class CustomerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f;
    public float stopDistance = 0.1f; // small distance for target reach

    private Transform targetTransform = null;
    private Vector3? targetPosition = null;

    private CustomerOrder order;
    private bool walking = false;

    void Awake()
    {
        order = GetComponent<CustomerOrder>();
    }

    void Update()
    {
        if (!walking) return;

        Vector3 currentTarget = targetTransform != null ? targetTransform.position : targetPosition ?? transform.position;

        float dist = Vector3.Distance(transform.position, currentTarget);

        if (dist > stopDistance)
        {
            Vector3 dir = currentTarget - transform.position;
            dir.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);

            if (dir.sqrMagnitude > 0.001f)
            {
                Quaternion lookRot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, 10f * Time.deltaTime);
            }
        }
        else
        {
            walking = false;

            // Reached the counter
            if (targetTransform != null)
            {
                if (order != null)
                    order.ShowOrder();
            }
            // Reached spawn/home
            else
            {
                Destroy(gameObject); // only destroy AFTER reaching spawn
            }
        }
    }

    public void SetTarget(Transform t)
    {
        targetTransform = t;
        targetPosition = null;
        walking = true;
    }

    public void SetTarget(Vector3 pos)
    {
        targetTransform = null;
        targetPosition = pos;
        walking = true;
    }

    public bool IsWalking()
    {
        return walking;
    }
}
