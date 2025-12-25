using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance { get; private set; }

    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration = 1f;

    private float fadeProgress = 1;
    private bool fading = false;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        float target = fading ? 1f : 0f;

        float step = Time.deltaTime / Mathf.Max(fadeDuration, Mathf.Epsilon);
        fadeProgress = Mathf.MoveTowards(fadeProgress, target, step);
        image.color = Color.Lerp(Color.clear, Color.black, fadeProgress);
    }

    public static void Load(string scene)
    {
        instance.fading = true;
        instance.RunAfter(instance.fadeDuration, () => {
            SceneManager.LoadScene(scene);
        });
    }
}
