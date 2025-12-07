using UnityEngine;

public class Orb : MonoBehaviour
{
    [SerializeField] private float gravity;
    [SerializeField] private float value = 15 / 4;
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.instance.orb.PlayAt(transform.position);
        GameManager.instance.player.AddMana(value);
        Destroy(gameObject);
    }
}
