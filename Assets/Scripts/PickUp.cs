using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform inHand; //held item space
    public KeyCode pickupKey = KeyCode.X;
    private GameObject objectInRange = null; //items in range that can be picked up
    private GameObject heldObject = null;    //object being held
    private bool isOverFrontCounter = false;
    public Transform frontCounterSpace;


    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("FrontCounter")) //if drop space is counter, set it on counter
        {
            isOverFrontCounter = true;
            frontCounterSpace = c.transform.Find("pizza");
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("FrontCounter")) 
        {
            isOverFrontCounter = false;
            frontCounterSpace = null;
        }
    }

    private void Update()
    {
        if (heldObject == null) //find a pickupable object
        {
            objectInRange = NearestPickup();
        }

        if (Input.GetKeyDown(pickupKey) && heldObject == null && objectInRange != null)
        {
            PickUp(objectInRange);
        }
        else if (Input.GetKeyDown(pickupKey) && heldObject != null)
        {
            DropHeldObject();
        }

    }

    private GameObject NearestPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Pickup"))
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearest = hit.gameObject;
                }
            }
        }

        return nearest;
    }
    private void PickUp(GameObject obj)
    {
        heldObject = Instantiate(obj, inHand.position, obj.transform.rotation); //instance of the ingredient
        heldObject.transform.SetParent(inHand); //makes sure it is a child of inHand

        Collider col = heldObject.GetComponent<Collider>(); 
        if (col != null) //prevents weird physics
        {
            col.enabled = false;
        }
    }
    private void DropHeldObject()
    {
        if (heldObject == null) return;

        heldObject.transform.SetParent(null);

        Collider col = heldObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        if (isOverFrontCounter && frontCounterSpace != null)
        {
            Ingredient ingredient = heldObject.GetComponent<Ingredient>();
            PizzaStack stack = frontCounterSpace.GetComponent<PizzaStack>();

            if (ingredient != null && stack != null) //checks if the ingredient can be stacked and what asset should appear on the counter
            {
                stack.TryAddIngredient(ingredient);
            }

            Destroy(heldObject); //gets rid of asset in hand
        }

        else
        {
            Destroy(heldObject);
        }


        heldObject = null;
    }
    public GameObject GetHeldObject()
    {
        return heldObject;
    }

    public bool IsHoldingObject()
    {
        return heldObject != null;
    }
}
