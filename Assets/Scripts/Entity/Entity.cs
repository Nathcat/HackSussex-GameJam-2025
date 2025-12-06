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

    private SpriteRenderer sprite;
    private float attackAnimation;
    private float walkAnimation;
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

        if (walkAnimation > 0)
        {
            sprite.transform.localPosition = new Vector2(0, 0.548f + Mathf.Sin(walkAnimation) * 0.5f);
            sprite.transform.localScale = new Vector2(12, 12 - Mathf.Cos(walkAnimation * 2));
            walkAnimation -= Time.deltaTime * animationSpeed;
        } else if (delta != Vector2.zero) walkAnimation = Mathf.PI;

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
        health += amount;
    }
}
