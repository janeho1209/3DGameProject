using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Transform inHand;
    public KeyCode pickupKey = KeyCode.X;
    public KeyCode deliverKey = KeyCode.C; // NEW: Delivery key

    private GameObject objectInRange = null;
    private GameObject heldObject = null;
    private bool isOverFrontCounter = false;
    public Transform frontCounterSpace;

    // NEW: Track if holding completed pizza
    private PizzaStack heldPizzaStack = null;

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("FrontCounter"))
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
        if (heldObject == null)
        {
            objectInRange = NearestPickup(); // This finds both "Pickup" and "CompletedPizza"
        }

        // Handle Pickup/Drop Logic (X Key)
        if (Input.GetKeyDown(pickupKey))
        {
            // When hand is empty and something is nearby
            if (heldObject == null && objectInRange != null)
            {
                PickUp(objectInRange);
            }
            // Holding something, press X to drop
            else if (heldObject != null)
            {
                DropHeldObject();
                heldPizzaStack = null;
            }
        }

        // Handle Delivery Logic (C Key)
        if (Input.GetKeyDown(deliverKey) && heldPizzaStack != null)
        {
            TryDeliverPizza();
        }
    }

    private GameObject NearestPickup()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Pickup") || hit.CompareTag("CompletedPizza"))
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
        PizzaStack pizzaStack = obj.GetComponent<PizzaStack>();
        if (pizzaStack == null) pizzaStack = obj.GetComponentInParent<PizzaStack>();

        // If it's a completed pizza, pick up the whole pizza
        if (pizzaStack != null && obj.CompareTag("CompletedPizza"))
        {
            heldPizzaStack = pizzaStack;
            heldObject = obj;

            // Parent it to hand
            heldObject.transform.SetParent(inHand);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;

            Collider col = heldObject.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            Debug.Log("Picked up a COMPLETED pizza!");
        }
        else
        {
            // existing logic for individual ingredients
            heldObject = Instantiate(obj, inHand.position, obj.transform.rotation);
            heldObject.transform.SetParent(inHand);
            heldObject.transform.localPosition = Vector3.zero;
            heldObject.transform.localRotation = Quaternion.identity;

            Collider col = heldObject.GetComponent<Collider>();
            if (col != null) col.enabled = false;

            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;

            heldPizzaStack = null;

            Debug.Log("Picked up a regular ingredient.");
        }
    }


    private void DetachHeldObject()
    {
        heldObject.transform.SetParent(null);

        Collider col = heldObject.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
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

    private void DropHeldObject()
    {
        if (heldObject == null) return;

        bool wasDestroyed = false;

        if (isOverFrontCounter && frontCounterSpace != null)
        {
            Ingredient ingredient = heldObject.GetComponent<Ingredient>();
            PizzaStack stack = frontCounterSpace.GetComponent<PizzaStack>();

            if (ingredient != null && stack != null)
            {
                stack.TryAddIngredient(ingredient);
                Destroy(heldObject);
                wasDestroyed = true;
            }
        }

        if (!wasDestroyed)
        {
            DetachHeldObject();
        }

        heldObject = null;
    }

    private void TryDeliverPizza()
    {
        if (heldPizzaStack == null) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, 3f);
        CustomerOrder nearestCustomer = null;
        float minDist = Mathf.Infinity;

        foreach (Collider hit in hits)
        {
            CustomerOrder customer = hit.GetComponent<CustomerOrder>();
            if (customer != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    nearestCustomer = customer;
                }
            }
        }

        if (nearestCustomer != null)
        {
            // Give the ingredients to the customer
            nearestCustomer.ReceivePizza(heldPizzaStack.GetIngredients());

            // Destroy the pizza object in hand
            Destroy(heldPizzaStack.gameObject);
            heldPizzaStack = null;
            heldObject = null;

            Debug.Log("Pizza delivered!");
        }
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
