using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private float multipler = 0.05f;
    private Vector3 initial;

    private void Start()
    {
        initial = transform.localPosition;
    }

    void Update()
    {
        transform.localPosition = new Vector3(
            -Camera.main.transform.position.x * multipler,
            -Camera.main.transform.position.y * multipler,
            transform.localPosition.z
        ) + initial;
    }
}
