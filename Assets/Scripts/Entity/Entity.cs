using UnityEngine;
using UnityEngine.Events;

public class Entity : MonoBehaviour
{
    [field: SerializeField] public float health { get; private set; }
    [SerializeField] public float maxHealth;
    [SerializeField] private float animationSpeed = 0.5f;

    public UnityEvent<float> onHeathChange = new UnityEvent<float>();
    public UnityEvent onDeath = new UnityEvent();
    public UnityEvent onMove = new UnityEvent();

    private bool rotationFixed = false;
    private SpriteRenderer sprite;
    private SpriteRenderer shadow;
    private float attackAnimation;
    private float walkAnimation;
    private Transform sprites;
    private Vector2 old = Vector2.zero;

    protected virtual void Start()
    {
        sprites = transform.Find("Sprites");
        shadow = sprites.Find("Shadow").GetComponent<SpriteRenderer>();
        sprite = sprites.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (old == Vector2.zero) old = transform.position;
        Vector2 position = transform.position;
        Vector2 delta = position - old;

        if (delta != Vector2.zero) onMove.Invoke();

        if (delta.x < 0) sprite.flipX = true;
        if (delta.x > 0) sprite.flipX = false;

        if (walkAnimation > 0)
        {
            sprite.transform.localPosition = Vector3.zero;
            sprite.transform.localPosition = new Vector3(0f, Mathf.Sin(walkAnimation) * 0.5f, 0f);
            sprite.transform.localScale = new Vector2(12, 12 - Mathf.Cos(walkAnimation * 2));
            shadow.transform.localScale = Vector3.one * (10 - Mathf.Sin(walkAnimation) * 3);
            walkAnimation -= Time.deltaTime * animationSpeed;
        } else if (delta != Vector2.zero)
        {
            walkAnimation = Mathf.PI;
            AudioManager.instance.walk.PlayAt(position);
        }

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

        int sorting = -Mathf.RoundToInt(shadow.transform.position.y * 10 - 3);
        sprite.sortingOrder = sorting;
        shadow.sortingOrder = sorting - 1;

        sprite.color = Color.Lerp(sprite.color, Color.white, Time.deltaTime * 4);

        old = position;
    }

    public void StartAttackAnimation()
    {
        attackAnimation = Mathf.PI;
    }

    public void AddHealth(float delta)
    {

        health = Mathf.Clamp(health + delta, 0, maxHealth);
        onHeathChange.Invoke(delta);

        if (delta < 0) sprite.color = Color.red;

        if (health == 0) {
            onDeath.Invoke();
        }
    }

    public void DestroyOnDie() {
        for (int i = 0; i < 4; i++) Instantiate(GameManager.instance.orbPrefab, sprites.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
