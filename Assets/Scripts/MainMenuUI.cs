using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Texture2D musicOn;
    [SerializeField] private Texture2D musicOff;
    private VisualElement splash;

    void Start() {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        splash = root.Q<VisualElement>("Splash");

        root.Q<Button>("Play").clicked += () => {
            LevelLoader.Load("Game");
        };

        root.Q<Button>("Quit").clicked += () => {
            Application.Quit();
        };

        root.Q<Button>("Credits").clicked += () => {
            Application.OpenURL("https://devpost.com/software/mind-your-mana");
        };

        Button music = root.Q<Button>("Music");
        music.clicked += () =>
        {
            Music.instance.muted = !Music.instance.muted;
            music.style.backgroundImage = Music.instance.muted
                ? Background.FromTexture2D(musicOff)
                : Background.FromTexture2D(musicOn);
        };
    }

    private void Update()
    {
        float bounce = 1.0f + 0.05f * Mathf.Sin(Time.time * 3.0f);
        splash.style.scale = new Vector3(bounce, bounce, 1.0f);
    }
}
