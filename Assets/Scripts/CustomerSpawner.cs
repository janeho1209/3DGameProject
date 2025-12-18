using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Prefabs")]
    public GameObject[] customerPrefabs; // Array of different customer models

    [Header("Spawn Settings")]
    public Transform spawnPoint;        // Where customers spawn
    public Transform customerTarget;    // Counter position
    public float spawnInterval = 4f;
    public int maxCustomers = 3;        // Max customers at once

    private int currentCustomerCount = 0;

    void Start()
    {
        //Debug.Log("CustomerSpawner Start called");
        //Debug.Log($"customerPrefabs: {customerPrefabs}, Length: {(customerPrefabs != null ? customerPrefabs.Length : 0)}");

        if (customerPrefabs == null || customerPrefabs.Length == 0)
        {
            //Debug.LogError("No customer prefabs assigned!");
            return;
        }

        //Debug.Log($"currentCustomerCount: {currentCustomerCount}, maxCustomers: {maxCustomers}");

        // Spawn first customer immediately
        if (currentCustomerCount < maxCustomers)
        {
            Debug.Log("Spawning first customer...");
            SpawnRandomCustomer();
        }

        // Start continuous spawning
        //Debug.Log("Starting spawn coroutine...");
        StartCoroutine(SpawnCustomersRoutine());
    }

    IEnumerator SpawnCustomersRoutine()
    {
        Debug.Log("Spawn coroutine started");
        // Wait before first continuous spawn
        yield return new WaitForSeconds(spawnInterval);

        while (true)
        {
            //Debug.Log($"Coroutine check: currentCustomerCount={currentCustomerCount}, maxCustomers={maxCustomers}");
            if (currentCustomerCount < maxCustomers)
            {
                SpawnRandomCustomer();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnRandomCustomer()
    {
        //Debug.Log("Spawning random customer...");
        GameObject randomPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        //Debug.Log($"Selected prefab: {randomPrefab}");
        SpawnCustomerAtPosition(randomPrefab);
    }

    private void SpawnCustomerAtPosition(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("Prefab is null!");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("Spawn point is null!");
            return;
        }

        //Debug.Log($"Instantiating customer at {spawnPoint.position}");
        GameObject customer = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
       // Debug.Log($"Customer instantiated: {customer.name}");

        // Set movement target
        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        if (movement != null)
        {
            Debug.Log("Found CustomerMovement component");
            if (customerTarget != null)
            {
                movement.SetTarget(customerTarget);
                Debug.Log($"Set target to: {customerTarget.name}");
            }
            else
            {
                Debug.LogError("Customer target is null!");
            }
        }
        else
        {
            Debug.LogError("No CustomerMovement component found on prefab!");
        }

        // Set spawn position so customer can return after delivery
        CustomerOrder order = customer.GetComponent<CustomerOrder>();
        if (order != null)
        {
            order.spawnPosition = spawnPoint.position;
            Debug.Log($"Set spawn position to: {spawnPoint.position}");
        }

        currentCustomerCount++;
       // Debug.Log($"Customer count increased to: {currentCustomerCount}");
        StartCoroutine(WaitForDespawn(customer));

        //Debug.Log($"Spawned customer. Active customers: {currentCustomerCount}");
    }

    IEnumerator WaitForDespawn(GameObject customer)
    {
        Debug.Log($"Waiting for despawn of {customer.name}");
        while (customer != null)
        {
            yield return null;
        }

        currentCustomerCount--;
        Debug.Log($"Customer left. Active customers: {currentCustomerCount}");
    }
}