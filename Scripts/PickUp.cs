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
        PizzaStack sourceStack = obj.GetComponent<PizzaStack>();
        if (sourceStack == null)
            sourceStack = obj.GetComponentInParent<PizzaStack>();

        if (sourceStack != null && sourceStack.CompareTag("CompletedPizza"))
        {
            GameObject pizzaCopy = Instantiate( //create copy for player's hand
                sourceStack.gameObject,
                inHand.position,
                Quaternion.identity
            );

            pizzaCopy.transform.SetParent(inHand, false);
            pizzaCopy.transform.localPosition = Vector3.zero;
            pizzaCopy.transform.localRotation = Quaternion.identity;
            pizzaCopy.transform.localScale = Vector3.one;

            if (pizzaCopy.TryGetComponent(out Collider col)) //prevents weird physics
                col.enabled = false;

            heldObject = pizzaCopy; //tracks held pizza
            heldPizzaStack = pizzaCopy.GetComponent<PizzaStack>();
            sourceStack.ResetPizza(); //clear counter

            return;
        }

        // PICK UP INGREDIENT (unchanged)
        heldObject = Instantiate(obj, inHand);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;

        if (heldObject.TryGetComponent(out Collider ingredientCol))
            ingredientCol.enabled = false;

        if (heldObject.TryGetComponent(out Rigidbody ingredientRb))
            ingredientRb.isKinematic = true;

        heldPizzaStack = null;

        Debug.Log("Picked up regular ingredient");
    }


    private void DropHeldObject()
    {
        if (heldObject == null) return;

        bool wasDestroyed = false;

        if (isOverFrontCounter && frontCounterSpace != null)
        {
            Debug.Log("trying to add ingredient");
            Ingredient ingredient = heldObject.GetComponent<Ingredient>();
            PizzaStack stack = frontCounterSpace.GetComponent<PizzaStack>();

            if (ingredient != null)
            {
                bool added = stack.TryAddIngredient(ingredient);

                if (added)
                {
                    Destroy(heldObject);
                    wasDestroyed = true;
                }
            }
        }

        if (!wasDestroyed)
        {
            Destroy(heldObject);
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
            //nearestCustomer.ReceivePizza(heldPizzaStack.GetIngredients());

            // Destroy the pizza object in hand
            nearestCustomer.ReceivePizza(heldPizzaStack.GetIngredients());
            heldPizzaStack.ResetPizza();
            //heldPizzaStack = null;
            //heldObject = null;

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
