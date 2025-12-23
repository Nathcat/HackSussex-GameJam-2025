using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BoltSpell : MonoBehaviour
{
    private float death = -1;
    private Rigidbody2D rb;
    private Light2D light;
    private float timer = 5;

    [SerializeField] private int bounces = 2;
    public float velocity = 1f;
    public float damage = 8f;

    private void Start()
    {
        light = transform.Find("Light 2D").GetComponent<Light2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision) {

        Entity c;
        if ((c = collision.transform.GetComponent<Entity>()) != null)
        {
            c.AddHealth(-damage);
            bounces = 0;
        }

        AudioManager.instance.bolt.PlayAt(transform.position);

        if (bounces > 1) {
            bounces--;
        } else End();
    }

    private void End()
    {
        Destroy(rb);
        GetComponent<AudioSource>().Stop();
        transform.Find("Particle Death").GetComponent<ParticleSystem>().Play();
        this.RunAfter(1f, () => Destroy(gameObject));
        death = Time.realtimeSinceStartup;
    }

    public void onSpellInvoked(Vector3 target) {
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        Vector2 v = new Vector2(direction.x, direction.y);
        v.Normalize();
        rb.linearVelocity = v * velocity;
    }

    private void Update()
    {
        if (death != -1) light.intensity = 5f - (Time.realtimeSinceStartup - death) * 5f;
        if (timer <= 0 && rb != null) End();
        timer -= Time.deltaTime;
    }
}
