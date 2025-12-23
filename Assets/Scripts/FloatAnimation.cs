using UnityEngine;

public class FloatAnimation : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.02f;
    Vector2 initial;

    private void Start()
    {
        initial = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = initial + new Vector2(0, Mathf.Sin(Time.realtimeSinceStartup) * amplitude);
    }
}
