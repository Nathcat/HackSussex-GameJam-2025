using UnityEngine;

public class PoolPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private float minDelay = 0.2f;
    [SerializeField] private float maxDelay = 0.8f;

    private float timer;

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (timer > 0) timer -= Time.deltaTime;
            else
            {
                audioSource.clip = clips[Random.Range(0, clips.Length)];
                timer = Random.Range(minDelay, maxDelay);
                audioSource.Play();
            }
        }
    }
}
