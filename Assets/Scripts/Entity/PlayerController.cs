using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Entity
{
    [SerializeField] private float walkSpeed = 10;

    [field: SerializeField] public float mana { get; private set; }
    [SerializeField] public float maxMana;
    [SerializeField] private float manaRegen;
    public readonly UnityEvent onManaChange = new UnityEvent();

    new private Rigidbody2D rigidbody;
    private InputAction sprintAction;
    private InputAction moveAction;

    protected override void Start()
    {
        base.Start();

        rigidbody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
    }

    protected override void Update()
    {
        base.Update();
        Vector2 velocity = moveAction.ReadValue<Vector2>() * walkSpeed;
        if (Application.isEditor && sprintAction.IsPressed()) velocity *= 5;
        rigidbody.linearVelocity = velocity;
        AddMana(manaRegen * Time.deltaTime);
    }

    public void OnDeath() {
        Debug.Log("Player died :(");
    }

    public void AddMana(float amount)
    {
        mana = Mathf.Clamp(mana + amount, 0, maxMana);
        onManaChange.Invoke();
    }
}
