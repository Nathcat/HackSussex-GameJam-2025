using UnityEngine;

public class SpellHint : MonoBehaviour
{
    [SerializeField] private float distance = 3;
    [field: SerializeField] public int[] hint { get; private set; }

    public bool inRange
    {
        get
        {
            return Vector2.Distance(transform.position, GameManager.instance.player.transform.position) <= distance;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
