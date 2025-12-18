using UnityEngine;

public class WholePizzaObject : MonoBehaviour
{
    public PizzaStack pizzaStackPrefab;
    public Transform spawnPoint;

    private PizzaStack currentStack;

    void Start()
    {
        SpawnNewStack();
    }

    public PizzaStack GetCurrentStack()
    {
        return currentStack;
    }

    public void ConsumeStack()
    {
        if (currentStack != null)
            Destroy(currentStack.gameObject);

        SpawnNewStack();
    }

    private void SpawnNewStack()
    {
        currentStack = Instantiate(
            pizzaStackPrefab,
            spawnPoint.position,
            Quaternion.identity,
            spawnPoint
        );
    }
}
