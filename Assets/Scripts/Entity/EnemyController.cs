using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : Entity {

    [SerializeField] private float speed = 9;

    new private Rigidbody2D rigidbody;
    private UnityEngine.AI.NavMeshAgent navAgent;

    override protected void Start()
    {
        base.Start();

        rigidbody = GetComponent<Rigidbody2D>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    override protected void Update()
    {
        Entity target = closestPlayer();
        navAgent.destination = target.transform.position;

        base.Update();

        transform.GetChild(0).localRotation = Quaternion.Inverse(transform.rotation);
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
