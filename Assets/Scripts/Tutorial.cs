using UnityEngine;

public class Tutorial : MonoBehaviour
{
    private readonly static Color transparent = new Color(1, 1, 1, 0);

    [SerializeField] private GameObject nextTutorial;

    new private SpriteRenderer renderer;
    private Color target = transparent;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        target = renderer.color;
        renderer.color = transparent;
    }

    private void Update()
    {
        renderer.color = Color.Lerp(renderer.color, target, Time.deltaTime * 5);
    }

    public void Complete()
    {
        if (gameObject.activeSelf == false || target == transparent) return;

        if (nextTutorial != null) nextTutorial.SetActive(true);
        AudioManager.instance.tutorial.PlayAt(transform.position);

        target = new Color(1, 1, 1, 0);
        this.RunAfter(1, () => Destroy(gameObject));
    }
}
