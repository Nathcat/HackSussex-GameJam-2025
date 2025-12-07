using UnityEngine;
using System.Collections;

public class HealingSpell : MonoBehaviour
{
    public float healAmount = 20f;
    public float timeToDestroy = 5f;
    public float radius = 5f;
    public float healingInterval = 0.1f;
    public float healingAmount = 10f;

    public void onSpellInvoked(Vector3 target) {
        transform.position = target;

        StartCoroutine(DestroyTimer());
    }

    private IEnumerator DestroyTimer() {
        float timeElapsed = 0f;
        Vector2 centre = new Vector2(transform.position.x, transform.position.y);

        while (timeElapsed < timeToDestroy) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                centre,
                radius
            );

            foreach (Collider2D c in colliders) {                
                Entity entity;
                if ((entity = c.GetComponent<Entity>()) != null) {
                    entity.AddHealth(healAmount);
                }
            }

            yield return new WaitForSeconds(healingInterval);
            timeElapsed += healingInterval;
        }
        Destroy(gameObject);
    }
}
