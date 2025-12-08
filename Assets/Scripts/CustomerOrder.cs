using UnityEngine;
using TMPro;

public class CustomerOrder : MonoBehaviour {
    [Header("Bubble Setup")]
    public GameObject speechBubblePrefab;
    public Transform bubbleAnchor; // Speech bubble position

    [Header("Order Text")]
    [TextArea]
    public string orderText = "I would like a pepperoni pizza!";
    private bool shown = false;

    public void ShowOrder() {
        if (shown) return;
        shown = true;

        if (speechBubblePrefab == null) {
            Debug.LogWarning("No speechBubblePrefab assigned on CustomerOrder.");
            return;
        }

        Transform parent = bubbleAnchor != null ? bubbleAnchor : transform;

        GameObject bubbleInstance = Instantiate(speechBubblePrefab, parent.position, Quaternion.identity, parent);

        Debug.Log("Speech bubble spawned.");

        TMP_Text text = bubbleInstance.GetComponentInChildren<TMP_Text>();
        if (text != null) {
            text.text = orderText;
        } else {
            Debug.LogWarning("No TMP_Text found in bubble prefab.");
        }

    }
    
}
