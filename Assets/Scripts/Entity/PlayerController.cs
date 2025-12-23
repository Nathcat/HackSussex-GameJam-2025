using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Entity
{
    [SerializeField] private float walkSpeed = 10;

    [field: SerializeField] public float mana { get; private set; }
    [SerializeField] public float maxMana;
    [SerializeField] private float manaRegen;

    [SerializeField] private Vector2 spawnOffset;
    public UnityEvent onManaChange = new UnityEvent();
    public UnityEvent onReady = new UnityEvent();
    public UnityEvent onMove = new UnityEvent();

    new private Rigidbody2D rigidbody;
    private InputAction sprintAction;
    private InputAction moveAction;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + (Vector3) spawnOffset, 0.2f);
    }

    protected override void Start()
    {
        base.Start();

        rigidbody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        onHeathChange.AddListener(HealthChanged);

        rigidbody.simulated = false;
        this.RunAfter(spawnOffset.magnitude / walkSpeed, () =>
        {
            rigidbody.simulated = true;
            onReady.Invoke();
        });
    }

    protected override void Update()
    {
        base.Update();
        if (!rigidbody.simulated) transform.position += Time.deltaTime * walkSpeed * (Vector3) spawnOffset.normalized;
        else
        {
            Vector2 velocity = moveAction.ReadValue<Vector2>() * walkSpeed;
            if (Application.isEditor && sprintAction.IsPressed()) velocity *= 5;
            rigidbody.linearVelocity = velocity;
            if (velocity.sqrMagnitude > 0) onMove.Invoke();
        }

        AddMana(manaRegen * Time.deltaTime);
    }

    public void OnDeath() {
        Debug.Log("Player died :(");
        SceneManager.LoadScene("GameOver");
    }

    public void AddMana(float amount)
    {
        mana = Mathf.Clamp(mana + amount, 0, maxMana);
        onManaChange.Invoke();
    }

    private void HealthChanged(float delta)
    {
        if (delta < 0) AudioManager.instance.hit.PlayAt(transform.position);
    }
}
