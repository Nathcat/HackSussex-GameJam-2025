using UnityEngine;
using UnityEngine.UIElements;

public class GameOverUI : MonoBehaviour
{
    private Button menuButton;

    void Start() {
        menuButton = GetComponent<UIDocument>().rootVisualElement.Q<Button>("ToMenuButton");
        menuButton.clicked += () => {
            LevelLoader.Load("MainMenu");
        };
    }
}
