using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyController : Entity {

    private NavMeshAgent navAgent;

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
}
