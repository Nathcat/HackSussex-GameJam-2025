using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : Entity {

    [SerializeField] private float speed = 9;

    new private Rigidbody2D rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector3 x = (closestPlayer().transform.position - transform.position).normalized;
        rigidbody.linearVelocity = x * speed;
    }

    private Entity closestPlayer()
    {
        Entity closest = null;
        float distance = float.PositiveInfinity;

        foreach (Entity entity in GameController.instance.players)
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
