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
    [SerializeField] private float teleportSpeed;
    [SerializeField] private GameObject teleportEffect;
    [SerializeField] private ParticleSystem teleportTrail;

    public UnityEvent onManaChange = new UnityEvent();
    public UnityEvent onReady = new UnityEvent();
    public UnityEvent onMove = new UnityEvent();

    new private Rigidbody2D rigidbody;
    private InputAction sprintAction;
    private InputAction moveAction;
    private Transform boss;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(transform.position + (Vector3) spawnOffset, 0.2f);
    }

    protected override void Start()
    {
        base.Start();

        boss = FindAnyObjectByType<BossEnemyController>().transform;
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
        else if (teleportTrail.isEmitting)
        {
            teleportTrail.transform.position += new Vector3(0, teleportSpeed, 0) * Time.deltaTime;
        } else {
            Vector2 velocity = moveAction.ReadValue<Vector2>() * walkSpeed;
            if (Application.isEditor && sprintAction.IsPressed()) velocity *= 5;
            rigidbody.linearVelocity = velocity;
            if (velocity.sqrMagnitude > 0) onMove.Invoke();
        }

        Camera.main.transform.localPosition = new Vector3(0, CameraHeight(), -10);

        AddMana(manaRegen * Time.deltaTime);
    }

    private float CameraHeight()
    {
        if (boss == null || transform.position.y < 150) return 0;

        float target = transform.position.y < boss.transform.position.y ? 2 : -2;
        return Mathf.Lerp(Camera.main.transform.localPosition.y, target, Time.deltaTime * 5);
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

    public void Teleport(System.Action onTeleport)
    {
        Instantiate(teleportEffect, transform.position, Quaternion.identity);
        AudioManager.instance.teleportOut.PlayAt(transform.position);
        sprites.gameObject.SetActive(false);
        invulnerable = true;

        var emission = teleportTrail.emission;
        emission.enabled = true;

        this.RunAfter(0.75f, () => {
            onTeleport();
            teleportSpeed = -teleportSpeed;
            AudioManager.instance.teleportIn.PlayAt(transform.position);
        });
        
        this.RunAfter(1.5f, () => {
            Instantiate(teleportEffect, transform.position, Quaternion.identity);
            var emission = teleportTrail.emission;
            emission.enabled = false;
            sprites.gameObject.SetActive(true);
            teleportSpeed = -teleportSpeed;
            invulnerable = false;
        });
    }
}
