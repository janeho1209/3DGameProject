using UnityEngine;

public class CustomerModelRandomizer : MonoBehaviour {
    [SerializeField] private Transform modelHolder;

    void Start() {
        int count = modelHolder.childCount;
        int randomIndex = Random.Range(0, count);

        for (int i = 0; i < count; i++) {
            modelHolder.GetChild(i).gameObject.SetActive(false);
        }

        modelHolder.GetChild(randomIndex).gameObject.SetActive(true);
    }

}
