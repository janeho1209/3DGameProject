using UnityEngine;
using System.Collections.Generic;

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

    // NEW: Track what ingredients are in the pizza
    private List<IngredientType> ingredients = new List<IngredientType>();

    public void TryAddIngredient(Ingredient ingredient)
    {
        switch (ingredient.type) //will only spawn on counter if stack has dough -> sauce -> cheese order
        {
            case IngredientType.Dough:
                if (!hasDough)
                {
                    Spawn(doughVisual);
                    hasDough = true;
                    ingredients.Add(IngredientType.Dough);
                }
                break;

            case IngredientType.Tomato:
                if (hasDough && !hasSauce)
                {
                    Spawn(sauceVisual);
                    hasSauce = true;
                    ingredients.Add(IngredientType.Tomato);
                }
                break;

            case IngredientType.Cheese:
                if (hasSauce && !hasCheese)
                {
                    Spawn(cheeseVisual);
                    hasCheese = true;
                    ingredients.Add(IngredientType.Cheese);
                }
                break;

            case IngredientType.Pepperoni:
                if (hasCheese)
                {
                    Spawn(pepperoniVisual);
                    ingredients.Add(IngredientType.Pepperoni);
                }
                break;
        }

        // NEW: Mark as completed pizza when it has cheese (minimum viable pizza)
        if (hasCheese && !gameObject.CompareTag("CompletedPizza"))
        {
            gameObject.tag = "CompletedPizza";
            Debug.Log("Pizza is ready to be picked up!");
        }
    }

    private void Spawn(GameObject prefab)
    {
        if (prefab == null) return;

        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform, worldPositionStays: false);
        go.transform.localPosition = Vector3.up * currentHeight;
        go.transform.localRotation = prefab.transform.rotation;
        currentHeight += ingredientHeight;
    }

    // NEW: Get list of ingredients for scoring
    public List<IngredientType> GetIngredients()
    {
        return new List<IngredientType>(ingredients);
    }

    public bool HasCheese()
    {
        return hasCheese;
    }

    public bool HasPepperoni()
    {
        return pepperoniVisual.activeSelf;
    }
}
