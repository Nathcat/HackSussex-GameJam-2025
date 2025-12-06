using UnityEngine;

public class BoltSpell : MonoBehaviour
{
    private Rigidbody2D rb;

    public float velocity = 1f;

    public void OnCollisionEnter2D(Collision2D collision) {
        // TODO Do something here!
        Destroy(gameObject);
    }

    public void onSpellInvoked(Vector3 target) {
        rb = GetComponent<Rigidbody2D>();
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        rb.linearVelocity = direction.normalized * velocity;
    }
}
