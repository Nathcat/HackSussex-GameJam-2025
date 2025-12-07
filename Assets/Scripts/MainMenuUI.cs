using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private Button playButton;

    void Start() {
        playButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("Play");
        playButton.clicked += () => {
            SceneManager.LoadScene("Game 1");
        };
    }
}
