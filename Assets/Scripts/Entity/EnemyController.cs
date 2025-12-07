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

    override protected void Start()
    {
        base.Start();
        navAgent = GetComponent<NavMeshAgent>();
    }

    override protected void Update()
    {
        Entity target = closestPlayer();
        navAgent.destination = target.transform.position;

        base.Update();

        Entity closest = closestPlayer();
        Vector2 cPos = new Vector2(closest.transform.position.x, closest.transform.position.y);
        Vector2 mPos = new Vector2(transform.position.x, transform.position.y);

        float d = (cPos - mPos).magnitude;
        if (d <= attackDistance) AttackEntity(closest);
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
}
