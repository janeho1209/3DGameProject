using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private GameObject howToPlayPanel;
    [SerializeField] private GameObject backgroundDimmer;

    public void PlayGame() {
        SceneManager.LoadScene("current iteration");
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void OpenHowToPlay() {
        backgroundDimmer.SetActive(true);
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay() {
        howToPlayPanel.SetActive(false);
        backgroundDimmer.SetActive(false);
    }
}
