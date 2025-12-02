using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform inHand;
    public float hitbox = 1f;
    public KeyCode pickupKey = KeyCode.X;
    private GameObject heldCopy = null;
    private GameObject objectToCopy = null;

    void Update()
    {
        if (objectToCopy == null)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, hitbox);
            foreach (Collider hit in hits)
            {
                if (hit.CompareTag("Pickup"))
                {
                    objectToCopy = hit.gameObject;
                    break;
                }
            }
        }

        if (Input.GetKey(pickupKey) && objectToCopy != null)
        {
            if (heldCopy == null)
            {
                heldCopy = Instantiate(objectToCopy);
            }

            heldCopy.transform.position = inHand.position;
            heldCopy.transform.rotation = inHand.rotation;
        }
        else
        {
            if (heldCopy != null)
            {
                Destroy(heldCopy);
                heldCopy = null;
                objectToCopy = null;
            }
        }
    }
}
