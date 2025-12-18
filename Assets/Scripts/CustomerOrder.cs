using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class CustomerOrder : MonoBehaviour
{

    [Header("Bubble Setup")]
    public GameObject speechBubblePrefab;
    public Transform bubbleAnchor;
    private GameObject activeBubble;

    [Header("Order Settings")]
    public IngredientType[] requiredIngredients; // What this customer wants
    private bool shown = false;
    private bool orderFulfilled = false;

    [Header("Order Text")]
    [TextArea]
    public string orderText = "I would like a pepperoni pizza!";

    [Header("Order Logic")]
    public IngredientType requestedIngredient;

    [Header("Emote Prefabs")]
    public GameObject happyEmotePrefab;   // Score 3
    public GameObject neutralEmotePrefab; // Score 2
    public GameObject sadEmotePrefab;     // Score 1
    public float emoteDuration = 2f;

    [Header("Emotes")]
    public Transform emoteAnchor;
    public GameObject happyEmote;
    public GameObject angryEmote;

    void Start()
    {
        if (requiredIngredients == null || requiredIngredients.Length == 0)
        {
            GenerateRandomOrder();
        }
    }

    public void GenerateRandomOrder()
    {
        // Level 1: Only Cheese or Pepperoni pizzas
        int randomOrder = Random.Range(0, 2);

        if (randomOrder == 0)
        {
            // Cheese Pizza: Dough, Tomato, Cheese
            requiredIngredients = new IngredientType[] {
                IngredientType.Dough,
                IngredientType.Tomato,
                IngredientType.Cheese
            };
            requestedIngredient = IngredientType.Cheese;
        }
        else
        {
            // Pepperoni Pizza: Dough, Tomato, Cheese, Pepperoni
            requiredIngredients = new IngredientType[] {
                IngredientType.Dough,
                IngredientType.Tomato,
                IngredientType.Cheese,
                IngredientType.Pepperoni
            };
            requestedIngredient = IngredientType.Pepperoni;
        }
    }

    public void ShowOrder()
    {
        if (shown) return;
        shown = true;

        if (speechBubblePrefab == null)
        {
            Debug.LogWarning("No speechBubblePrefab assigned on CustomerOrder.");
            return;
        }

        Transform parent = bubbleAnchor != null ? bubbleAnchor : transform;

        orderText = requestedIngredient == IngredientType.Pepperoni
        ? "I would like a pepperoni pizza!"
        : "I would like a cheese pizza!";

        activeBubble = Instantiate(speechBubblePrefab, parent.position, Quaternion.identity, parent);

        TMP_Text text = activeBubble.GetComponentInChildren<TMP_Text>();
        if (text != null)
        {
            text.text = orderText;
        }
    }

    public void ReceivePizza(List<IngredientType> deliveredIngredients)
    {
        if (orderFulfilled) return;
        orderFulfilled = true;

        int score = CalculateScore(deliveredIngredients);
        ShowEmote(score);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(score);
        }

        Invoke("DespawnCustomer", emoteDuration);
    }

    public void ReceivePizza(PizzaStack pizza)
    {
        bool correct = CheckPizza(pizza);
        ShowEmote(correct ? 3 : 1);
        Destroy(pizza.gameObject);
        Invoke("DespawnCustomer", emoteDuration);
    }

    int CalculateScore(List<IngredientType> delivered)
    {
        // Perfect match = 3 stars (Happy)
        // Partial match = 2 stars (Neutral)  
        // Wrong/missing = 1 star (Sad)

        if (delivered.Count != requiredIngredients.Length)
        {
            return 1;
        }

        int matchCount = 0;
        foreach (IngredientType required in requiredIngredients)
        {
            if (delivered.Contains(required))
            {
                matchCount++;
            }
        }

        if (matchCount == requiredIngredients.Length)
        {
            return 3;
        }
        else if (matchCount >= requiredIngredients.Length - 1)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    bool CheckPizza(PizzaStack pizza)
    {
        if (requestedIngredient == IngredientType.Cheese)
            return pizza.HasCheese();

        if (requestedIngredient == IngredientType.Pepperoni)
            return pizza.HasPepperoni();

        return false;
    }

    void ShowEmote(int score)
    {
        if (activeBubble != null)
        {
            Destroy(activeBubble);
        }

        GameObject emotePrefab = null;
        switch (score)
        {
            case 3:
                emotePrefab = happyEmotePrefab;
                break;
            case 2:
                emotePrefab = neutralEmotePrefab;
                break;
            case 1:
                emotePrefab = sadEmotePrefab != null ? sadEmotePrefab : angryEmote;
                break;
        }

        if (emotePrefab != null)
        {
            Transform parent = emoteAnchor != null ? emoteAnchor : transform;
            Instantiate(emotePrefab, parent.position, Quaternion.identity, parent);
        }
    }

    void DespawnCustomer()
    {
        Destroy(gameObject);
    }
}
