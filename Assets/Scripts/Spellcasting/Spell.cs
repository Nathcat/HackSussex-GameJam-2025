using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class Spell : MonoBehaviour
{
    public UnityEvent<Vector3> onSpellInvoked = new UnityEvent<Vector3>();

    [SerializeField]
    private int[] castPattern;

    [field: SerializeField]
    public float manaCost { get; private set; }

    public virtual bool MatchPattern(int[] pattern) {
        for (int i = 0; i < 6; i++)
            if (Enumerable.SequenceEqual(castPattern, pattern.Select(s => (s + i) % 6))) return true;

        return false;
    }

    public virtual Vector3 DetermineSpellTarget(Vector3 start, int[] pattern, Vector3 end) {
        return start;
    }

    public void SpellInvokedDebugMessage(Vector3 p) {
        Debug.Log(gameObject.name + " invoked at " + p);
    }
}
