using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        gameObject.SetActive(true);
        transform.SetParent(null);
        transform.localScale = Vector3.one;
    }

    void Update()
    {
        if (particles.particleCount == 0) Destroy(gameObject);
    }

    public static void Activate(GameObject gameObject)
    {
        gameObject.GetComponent<ParticlePlayer>().Start();
    }
}
