using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Prefabs")]
    public GameObject[] customerPrefabs; // Array of different customer models
    public GameObject customerPrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;        // Where customers spawn
    public Transform customerTarget;    // Counter position
    public float spawnInterval = 4f;
    public int maxCustomers = 3;        // Max customers at once

    private int currentCustomerCount = 0;

    void Start()
    {
        if (customerPrefabs != null && customerPrefabs.Length > 0)
        {
            StartCoroutine(SpawnCustomersRoutine());
        }
        else
        {
            SpawnCustomer();
        }
    }

    IEnumerator SpawnCustomersRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentCustomerCount < maxCustomers)
            {
                SpawnRandomCustomer();
            }
        }
    }

    public void SpawnRandomCustomer()
    {
        if (customerPrefabs == null || customerPrefabs.Length == 0)
        {
            Debug.LogError("No customer prefabs assigned!");
            return;
        }

        GameObject randomPrefab = customerPrefabs[Random.Range(0, customerPrefabs.Length)];
        SpawnCustomerAtPosition(randomPrefab);
    }

    public void SpawnCustomer()
    {
        if (customerPrefab == null)
        {
            Debug.LogError("No customer prefab assigned!");
            return;
        }

        SpawnCustomerAtPosition(customerPrefab);
    }

    private void SpawnCustomerAtPosition(GameObject prefab)
    {
        GameObject customer = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);

        // Set movement target
        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        if (movement != null)
        {
            movement.SetTarget(customerTarget);
        }

        // Set spawn position so customer can return after delivery
        CustomerOrder order = customer.GetComponent<CustomerOrder>();
        if (order != null)
        {
            order.spawnPosition = spawnPoint.position;
        }

        currentCustomerCount++;
        StartCoroutine(WaitForDespawn(customer));

        Debug.Log($"Spawned customer. Active customers: {currentCustomerCount}");
    }

    IEnumerator WaitForDespawn(GameObject customer)
    {
        while (customer != null)
        {
            yield return null;
        }

        currentCustomerCount--;
        Debug.Log($"Customer left. Active customers: {currentCustomerCount}");
    }
}
