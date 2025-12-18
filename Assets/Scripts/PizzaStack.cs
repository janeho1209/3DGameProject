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

    public float currentHeight = 0f;
    public float ingredientHeight = 1.0f;

    // NEW: Track what ingredients are in the pizza
    private List<IngredientType> ingredients = new List<IngredientType>();

    public bool TryAddIngredient(Ingredient ingredient)
    {
        Debug.Log($"Stack position: {transform.position}, height: {currentHeight}");

        bool added = false;

        switch (ingredient.type)
        {
            case IngredientType.Dough:
                if (!hasDough)
                {
                    Spawn(doughVisual);
                    hasDough = true;
                    ingredients.Add(IngredientType.Dough);
                    added = true;
                }
                break;

            case IngredientType.Tomato:
                if (hasDough && !hasSauce)
                {
                    Spawn(sauceVisual);
                    hasSauce = true;
                    ingredients.Add(IngredientType.Tomato);
                    added = true;
                }
                break;

            case IngredientType.Cheese:
                if (hasSauce && !hasCheese)
                {
                    Spawn(cheeseVisual);
                    hasCheese = true;
                    ingredients.Add(IngredientType.Cheese);
                    added = true;
                }
                break;

            case IngredientType.Pepperoni:
                if (hasCheese)
                {
                    Spawn(pepperoniVisual);
                    ingredients.Add(IngredientType.Pepperoni);
                    added = true;
                }
                break;
        }

        if (added && hasDough && !CompareTag("CompletedPizza"))
        {
            gameObject.tag = "CompletedPizza";
            Debug.Log("Pizza is ready to be picked up!");
        }

        return added;
    }


    private void Spawn(GameObject prefab)
    {
        GameObject go = Instantiate(prefab);
        go.transform.SetParent(transform, worldPositionStays: true);
        go.transform.position = transform.position + Vector3.up * currentHeight; //place on top of counter/other ingredients
        go.transform.rotation = prefab.transform.rotation; //preserve the original rotation
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

    public void ResetPizza()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        hasDough = false;
        hasSauce = false;
        hasCheese = false;

        ingredients.Clear();
        currentHeight = 0f;

        gameObject.tag = "Untagged";
    }

}
