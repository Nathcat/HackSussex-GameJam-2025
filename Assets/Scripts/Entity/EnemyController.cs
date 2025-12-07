using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : Entity {

    private NavMeshAgent navAgent;
    [SerializeField]
    private float attackCooldown = 2f;
    [SerializeField]
    private float attackDistance = 0.5f;
    private bool attackOnCooldown = false;
    [SerializeField]
    private float attackDamage = 20f;
    [SerializeField]
    private AggroGroup aggroGroup;
    [SerializeField]
    private float eyesightDistance = 10f;
    [SerializeField]
    private GameObject healthBar;

    override protected void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.destination = new Vector3(transform.position.x, transform.position.y, 0f);

        base.onHeathChange.AddListener(UpdateHealthBar);
    }

    override protected void Update()
    {
        if (aggroGroup == null) {
            Debug.LogWarning(gameObject.name + " has no aggro group!");
            base.Update();
            return;
        }

        if (aggroGroup.target == null) {
            foreach (Entity p in GameManager.instance.players) {
                if (IsInEyeline(p.gameObject.GetComponent<BoxCollider2D>())) {
                    Debug.Log(p.gameObject.name + " is in " + gameObject.name + "'s eyeline!");
                    aggroGroup.aggroEvent.Invoke(p);
                }
            }

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

    private Entity closestPlayer()
    {
        Entity closest = null;
        float distance = float.PositiveInfinity;

        foreach (Entity entity in GameManager.instance.players)
        {
            float d = (transform.position - entity.transform.position).sqrMagnitude;
            if (d < distance)
            {
                closest = entity;
                distance = d;
            }
        }

        return closest;
    }

    protected void AttackEntity(Entity player) {
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
        RaycastHit2D hit = Physics2D.Raycast(p, vp - p, eyesightDistance, LayerMask.GetMask("Player"));

        if (hit) {
            Debug.Log(hit.collider);
            return hit.collider == v && hit.distance <= eyesightDistance;
        }
        else return false;
    }

    private void UpdateHealthBar() {
        float fraction = health / maxHealth;
        healthBar.transform.localScale = new Vector3(2 * fraction, 0.2f, 0.2f);
    }
}
