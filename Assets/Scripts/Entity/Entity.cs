using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public GUID Guid { get; private set; }
    [field: SerializeField] public float health { get; private set; }
    [SerializeField] public float maxHealth;
    [SerializeField] private float animationSpeed = 0.5f;

    public readonly UnityEvent onHeathChange = new UnityEvent();
    public UnityEvent onDeath = new UnityEvent();

    private bool rotationFixed = false;
    private SpriteRenderer sprite;
    private float attackAnimation;
    private float walkAnimation;
    private Transform sprites;
    private Transform shadow;
    private Vector2 old;

    protected virtual void Start()
    {
        Guid = GUID.Generate();
        old = transform.position;

        sprites = transform.Find("Sprites");
        shadow = sprites.Find("Shadow").transform;
        sprite = sprites.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        Vector2 position = transform.position;
        Vector2 delta = position - old;

        if (delta.x < 0) sprite.flipX = true;
        if (delta.x > 0) sprite.flipX = false;

        if (walkAnimation > 0)
        {
            sprite.transform.localPosition = Vector3.zero;
            sprite.transform.localPosition = new Vector3(0f, Mathf.Sin(walkAnimation) * 0.5f, 0f);
            sprite.transform.localScale = new Vector2(12, 12 - Mathf.Cos(walkAnimation * 2));
            shadow.localScale = Vector3.one * (10 - Mathf.Sin(walkAnimation) * 3);
            walkAnimation -= Time.deltaTime * animationSpeed;
        } else if (delta != Vector2.zero) walkAnimation = Mathf.PI;

        if (!rotationFixed)
        {
            sprites.localRotation = Quaternion.Inverse(transform.rotation);
            rotationFixed = false;
        }

        if (attackAnimation > 0)
        {
            sprite.transform.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(attackAnimation) * 15 * (sprite.flipX ? 1 : -1));
            attackAnimation -= Time.deltaTime * animationSpeed;
        }

        old = position;
    }

    public void StartAttackAnimation()
    {
        attackAnimation = Mathf.PI;
    }

    public void AddHealth(float amount)
    {

        health = Mathf.Clamp(health + amount, 0, maxHealth);
        onHeathChange.Invoke();

        if (health == 0) {
            onDeath.Invoke();
        }
    }

    public void DestroyOnDie() {
        Destroy(gameObject);
    }
}
