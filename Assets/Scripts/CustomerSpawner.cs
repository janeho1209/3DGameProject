using UnityEngine;
using System.Collections;

public class CustomerSpawner : MonoBehaviour
{
    [Header("Customer Prefabs")]
    public GameObject[] customerPrefabs; // Array of different customer models
    public GameObject customerPrefab;

    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public Transform customerTarget; // Counter position
    public float spawnInterval = 4f;
    public int maxCustomers = 3; // Max customers at once

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
        GameObject customer = Instantiate(randomPrefab, spawnPoint.position, spawnPoint.rotation);

        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        if (movement != null)
        {
            movement.target = customerTarget;
        }

        currentCustomerCount++;
        StartCoroutine(WaitForDespawn(customer));

        Debug.Log($"Spawned customer. Active customers: {currentCustomerCount}");
    }

    public void SpawnCustomer()
    {
        GameObject customer = Instantiate(customerPrefab, spawnPoint.position, spawnPoint.rotation);

        CustomerMovement movement = customer.GetComponent<CustomerMovement>();
        movement.target = customerTarget;

        currentCustomerCount++;
        StartCoroutine(WaitForDespawn(customer));
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
