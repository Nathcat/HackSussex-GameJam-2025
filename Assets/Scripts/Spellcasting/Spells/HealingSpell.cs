using UnityEngine;
using System.Collections;

public class HealingSpell : MonoBehaviour
{
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
                // TODO Do adding health here!
                // Must find the component which has the game object's health, if it has that component,
                // and then add health to it
                Debug.LogWarning("Health not implemented!");
                
                Entity entity;
                if ((entity = c.GetComponent<Entity>()) != null) {
                    
                }
            }

            yield return new WaitForSeconds(healingInterval);
            timeElapsed += healingInterval;
        }
        Destroy(gameObject);
    }
}
