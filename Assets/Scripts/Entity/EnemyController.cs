using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : Entity {

    protected NavMeshAgent navAgent;
    [SerializeField]
    protected float attackCooldown = 2f;
    [SerializeField]
    protected float attackDistance = 0.5f;
    protected bool attackOnCooldown = false;
    [SerializeField]
    protected float attackDamage = 20f;
    [SerializeField]
    protected AggroGroup aggroGroup;
    private bool aggroDelay = false;
    [SerializeField]
    protected float eyesightDistance = 10f;
    [SerializeField]
    protected GameObject healthBar;

    override protected void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.destination = new Vector3(transform.position.x, transform.position.y, 0f);

        base.onHeathChange.AddListener(UpdateHealthBar);
        aggroGroup.aggroEvent.AddListener(OnAggro);
    }

    override protected void Update()
    {
        if (aggroGroup == null) {
            Debug.LogWarning(gameObject.name + " has no aggro group!");
            base.Update();
            return;
        }

        if (aggroGroup.target == null) {
            PlayerController player = GameManager.instance.player;
            if (IsInEyeline(player.gameObject.GetComponent<Collider2D>())) {
                Debug.Log(player.gameObject.name + " is in " + gameObject.name + "'s eyeline!");
                aggroGroup.aggroEvent.Invoke(player);
            }

            base.Update();

            return;
        }

        if (aggroDelay == true) {
            base.Update();
            return;
        }

        navAgent.destination = aggroGroup.target.transform.position;

        base.Update();

        Vector2 cPos = new Vector2(aggroGroup.target.transform.position.x, aggroGroup.target.transform.position.y);
        Vector2 mPos = new Vector2(transform.position.x, transform.position.y);

        float d = (cPos - mPos).magnitude;
        if (d <= attackDistance) AttackEntity(aggroGroup.target);
    }

    protected virtual void AttackEntity(Entity player) {
        if (!attackOnCooldown) {
            base.StartAttackAnimation();
            player.AddHealth(-attackDamage);
            attackOnCooldown = true;
            navAgent.isStopped = true;

            this.RunAfter(attackCooldown, () => { attackOnCooldown = false; navAgent.isStopped = false; });
        }
    }

    private bool IsInEyeline(Collider2D v) {
        Vector2 p = new Vector2(transform.position.x, transform.position.y);
        Vector2 vp = new Vector2(v.transform.position.x, v.transform.position.y);
        RaycastHit2D hit = Physics2D.Raycast(p, vp - p, eyesightDistance, -1 ^ LayerMask.GetMask("Enemy"));

        if (hit) {
            return hit.collider == v && hit.distance <= eyesightDistance;
        }
        else return false;
    }

    private void UpdateHealthBar() {
        float fraction = health / maxHealth;
        healthBar.transform.localScale = new Vector3(2 * fraction, 0.2f, 0.2f);
        aggroGroup.aggroEvent.Invoke(GameManager.instance.player);
    }

    private void OnAggro(Entity source) {
        aggroDelay = true;
        this.RunAfter(Random.Range(0f, 5f), () => { aggroDelay = false; }); 
    }
}
