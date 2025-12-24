using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HealingSpell : MonoBehaviour
{
    public float healAmount = 20f;
    public float timeToDestroy = 5f;
    public float radius = 5f;
    public float healingInterval = 0.1f;
    public float healingAmount = 10f;

    private float start;
    private Light2D light;

    private void Start()
    {
        start = Time.realtimeSinceStartup;
        light = transform.Find("Light 2D").GetComponent<Light2D>();
    }

    public void onSpellInvoked(Vector3 target) {
        transform.position = target;
        AudioManager.instance.heal.PlayAt(target);
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
                PlayerController entity;
                if ((entity = c.GetComponent<PlayerController>()) != null) {
                    entity.AddHealth(healAmount);
                }
            }

            yield return new WaitForSeconds(healingInterval);
            timeElapsed += healingInterval;
        }
        Destroy(gameObject);
    }

    private void Update()
    {
        light.intensity = 10f - (Time.realtimeSinceStartup - start) * 2f;
    }
}
