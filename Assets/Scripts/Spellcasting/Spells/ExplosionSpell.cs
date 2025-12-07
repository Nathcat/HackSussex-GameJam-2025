using UnityEngine;

public class ExplosionSpell : MonoBehaviour
{
    public float radius = 5f;
    public float baseDamage = 25f;
    public AnimationCurve damageFalloff;

    public void onSpellInvoked(Vector3 target) {
        transform.position = target;
        AudioManager.instance.explosion.PlayAt(target);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x, transform.position.y),
            radius
        );

        foreach (Collider2D c in colliders) {
            Entity entity;
            if ((entity = c.GetComponent<Entity>()) != null) {
                // Again, STUPID
                
                Vector2 e = new Vector2(entity.transform.position.x, entity.transform.position.y);
                Vector2 t = new Vector2(transform.position.x, transform.position.y);

                float distance = (e - t).magnitude;
                if (distance <= radius) {   // WHY DO I NEED TO DO THIS UNITY IS SO FUCKING STUPID
                    Debug.Log(damageFalloff.Evaluate(distance / radius));
                    float damage = baseDamage * damageFalloff.Evaluate(distance / radius);
                    entity.AddHealth(-damage);
                }
            }
        }

        this.RunAfter(5f, () => { Destroy(gameObject); });
    }
}
