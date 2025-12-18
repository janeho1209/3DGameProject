using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class CustomerOrder : MonoBehaviour
{
    [Header("Bubble Setup")]
    public GameObject speechBubblePrefab;
    public Transform bubbleAnchor;

    [Header("Order Settings")]
    public IngredientType[] requiredIngredients;
    private bool shown = false;
    private bool orderFulfilled = false;

    [Header("Order Text")]
    [TextArea]
    public string orderText = "I would like a pizza!";

    [Header("Spawn Tracking")]
    public Vector3 spawnPosition;

    [Header("Emote Prefabs")]
    public GameObject happyEmotePrefab;   // 0 mistakes
    public GameObject neutralEmotePrefab; // 1 mistake
    public GameObject angryEmotePrefab;   // 2+ mistakes
    public Transform emoteAnchor;

    [HideInInspector]
    public GameObject activeBubble;

    void Start()
    {
        if (requiredIngredients == null || requiredIngredients.Length == 0)
            GenerateRandomOrder();
    }

    public void GenerateRandomOrder()
    {
        int randomOrder = Random.Range(0, 2);

        if (randomOrder == 0)
        {
            requiredIngredients = new IngredientType[]
            {
                IngredientType.Dough,
                IngredientType.Tomato,
                IngredientType.Cheese
            };
            orderText = "I would like a cheese pizza!";
        }
        else
        {
            requiredIngredients = new IngredientType[]
            {
                IngredientType.Dough,
                IngredientType.Tomato,
                IngredientType.Cheese,
                IngredientType.Pepperoni
            };
            orderText = "I would like a pepperoni pizza!";
        }
    }

    public void ShowOrder()
    {
        if (shown) return;
        shown = true;

        if (speechBubblePrefab == null) return;

        Transform parent = bubbleAnchor != null ? bubbleAnchor : transform;
        activeBubble = Instantiate(speechBubblePrefab, parent.position, Quaternion.identity, parent);

        TMP_Text text = activeBubble.GetComponentInChildren<TMP_Text>();
        if (text != null)
            text.text = orderText;
    }

    public void ReceivePizza(List<IngredientType> deliveredIngredients)
    {
        if (orderFulfilled) return;
        orderFulfilled = true;

        int score = CalculateScore(deliveredIngredients);
        Debug.Log($"Score: {score}");
        ShowEmote(score);

        if (GameManager.Instance != null)
            GameManager.Instance.AddScore(score);

        WalkBackHome();
    }

    private int CalculateScore(List<IngredientType> delivered)
    {
        int mistakes = 0;

        // Count missing ingredients
        foreach (IngredientType req in requiredIngredients)
        {
            if (!delivered.Contains(req))
                mistakes++;
        }

        // Count extra ingredients
        mistakes += Mathf.Max(0, delivered.Count - requiredIngredients.Length);

        if (mistakes == 0) return 3; // happy
        if (mistakes == 1) return 2; // neutral
        return 1;                     // angry
    }

    private void ShowEmote(int score)
    {
        if (activeBubble != null)
            Destroy(activeBubble);

        GameObject prefab = null;
        Debug.Log($"Score: {score}");
        switch (score)
        {
            case 3: prefab = happyEmotePrefab; break;
            case 2: prefab = neutralEmotePrefab; break;
            case 1: prefab = angryEmotePrefab; break;
        }

        if (prefab != null)
        {
            Transform parent = emoteAnchor != null ? emoteAnchor : transform;
            activeBubble = Instantiate(prefab, parent.position, Quaternion.identity, parent);
        }
    }

    public void WalkBackHome()
    {
        CustomerMovement movement = GetComponent<CustomerMovement>();
        if (movement != null)
        {
            movement.SetTarget(spawnPosition);
        }

        // Keep emote active until the customer reaches spawn
        StartCoroutine(WaitUntilHome());
    }

    private IEnumerator WaitUntilHome()
    {
        CustomerMovement movement = GetComponent<CustomerMovement>();
        if (movement == null) yield break;

        // Wait until the customer actually stops moving
        while (movement.IsWalking())
        {
            yield return null;
        }

        // Remove emote
        if (activeBubble != null)
            Destroy(activeBubble);

        // Finally destroy customer
        Destroy(gameObject);
    }

}