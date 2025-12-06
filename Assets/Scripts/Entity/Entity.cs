using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public GUID Guid { get; private set; }
    [field: SerializeField] public float Health { get; private set; }
    [SerializeField] public float MaxHeath;
    [SerializeField] private float animationSpeed = 0.5f;

    public readonly UnityEvent onHeathChange = new UnityEvent();

    private SpriteRenderer sprite;
    private float animation;
    private Vector2 old;

    protected virtual void Start()
    {
        Guid = GUID.Generate();
        old = transform.position;
        sprite = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Vector2 position = transform.position;
        Vector2 delta = position - old;

        if (delta.x < 0) sprite.flipX = true;
        if (delta.x > 0) sprite.flipX = false;

        if (animation > 0)
        {
            sprite.transform.localPosition = new Vector2(0, Mathf.Sin(animation) * 0.5f);
            sprite.transform.localScale = new Vector2(12, 12 - Mathf.Cos(animation * 2));
            animation -= Time.deltaTime * animationSpeed;
        } else if (delta != Vector2.zero) animation = Mathf.PI;

        old = position;
    }
}
