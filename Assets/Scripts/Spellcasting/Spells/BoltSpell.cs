using UnityEngine;

public class BoltSpell : MonoBehaviour
{
    private Rigidbody2D rb;

    public float velocity = 1f;
    public float damage = 8f;

    public void OnCollisionEnter2D(Collision2D collision) {

        Entity c;
        if ((c = collision.transform.GetComponent<Entity>()) != null) c.AddHealth(-damage);

        Destroy(rb);
        AudioManager.instance.bolt.PlayAt(transform.position);
        GetComponent<AudioSource>().Stop();
        transform.Find("Particle Death").GetComponent<ParticleSystem>().Play();
        this.RunAfter(1f, () => Destroy(gameObject));
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
}
