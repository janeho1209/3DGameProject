using UnityEngine;

public class CustomerSpawner : MonoBehaviour {
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public Transform customerTarget;

    void Start() {
        SpawnCustomer();
    }

    public void SpawnCustomer() {
        GameObject customer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        movement.target = customerTarget;
    }

}
