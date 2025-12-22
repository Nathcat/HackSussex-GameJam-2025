using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchFlicker : MonoBehaviour
{
    private Light2D light;
    private float target;
    private float initial;
    private float speed;
    
    void Start()
    {
        light = GetComponent<Light2D>();
        initial = light.intensity;
        Choose();
    }

    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, target, speed);
    }

    void Choose()
    {
        speed = Random.Range(0.01f, 0.1f);
        target = initial + Random.Range(-1f, 0.5f);
        this.RunAfter(Random.Range(0.2f, 0.8f), Choose);
    }
}
