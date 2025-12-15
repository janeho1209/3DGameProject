using UnityEngine;

public class PizzaStack : MonoBehaviour
{
    public GameObject doughVisual;
    public GameObject sauceVisual;
    public GameObject cheeseVisual;
    public GameObject pepperoniVisual;

    private bool hasDough = false;
    private bool hasSauce = false;
    private bool hasCheese = false;

    private float currentHeight = 0f;
    public float ingredientHeight = 1.0f;

    public void TryAddIngredient(Ingredient ingredient)
    {
        switch (ingredient.type)
        {
            case IngredientType.Dough:
                if (!hasDough)
                {
                    Spawn(doughVisual);
                    hasDough = true;
                }
                break;

            case IngredientType.Tomato:
                if (hasDough && !hasSauce)
                {
                    Spawn(sauceVisual);
                    hasSauce = true;
                }
                break;

            case IngredientType.Cheese:
                if (hasSauce && !hasCheese)
                {
                    Spawn(cheeseVisual);
                    hasCheese = true;
                }
                break;

            case IngredientType.Pepperoni:
                if (hasCheese)
                {
                    Spawn(pepperoniVisual);
                }
                break;
        }
    }

    private void Spawn(GameObject prefab)
    {
        Vector3 spawnPos = transform.position + Vector3.up * currentHeight;
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform, worldPositionStays: true); // keep world scale/rotation
        go.transform.position = transform.position + Vector3.up * currentHeight; // place on top of counter
        go.transform.rotation = prefab.transform.rotation;
        currentHeight += ingredientHeight;
    }
}
