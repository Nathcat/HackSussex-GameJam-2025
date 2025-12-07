using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    private Button menuButton;

    void Start() {
        menuButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("ToMenuButton");
        menuButton.clicked += () => {
            SceneManager.LoadScene("MainMenu");
        };
    }
}
