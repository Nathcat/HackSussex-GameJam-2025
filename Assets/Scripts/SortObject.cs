using UnityEngine;

public class SortObject : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -Mathf.RoundToInt(transform.position.y * 10);
    }
}
