using UnityEngine;
using UnityEngine.AI;

public class Orb : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float manaValue = 35 / 5;
    [SerializeField] private float healthValue = 10 / 5;
    [SerializeField] private AnimationCurve magnetismCurve;

    private bool landed = false;
    private Vector3 velocity;
    private float floor;

    private void Start()
    {
        velocity = new Vector3(Random.Range(-2, 2), Random.Range(5, 10), 0);
        floor = transform.position.y + Random.Range(-0.5f, 0.5f);
    }

    private void Update()
    {
        if (transform.position.y > floor || velocity.y > 0)
        {
            transform.position += velocity * Time.deltaTime;
            velocity = new Vector3(velocity.x, velocity.y - gravity * Time.deltaTime, 0);
        } else if (!landed)
        {
            landed = true;
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, float.PositiveInfinity, NavMesh.AllAreas)) {
                Debug.Log("Orb landed at " + hit.position);
                transform.position = hit.position;
            }
        }

        float distance = Vector2.Distance(transform.position, GameManager.instance.player.transform.position);
        Vector3 direction = (GameManager.instance.player.transform.position - transform.position).normalized;
        float magnetism = magnetismCurve.Evaluate(distance);
        transform.position += direction * magnetism * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.instance.orb.PlayAt(transform.position);
        GameManager.instance.player.AddMana(manaValue);
        GameManager.instance.player.AddHealth(healthValue);
        Destroy(gameObject);
    }
}
