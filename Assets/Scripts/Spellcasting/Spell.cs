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
        int[] flippedPattern = castPattern.Select((s) => 5 - s).ToArray();
        return PatternsMatch(castPattern, pattern) || PatternsMatch(flippedPattern, pattern);
    }

    private bool PatternsMatch(int[] patternA, int[] patternB) {
        for (int i = 0; i < patternA.Length; i++)
            if (Enumerable.SequenceEqual(patternB, patternA.Select((_, j) => patternA[(i + j) % patternA.Length]))) return true;
        return false;
    }

    public virtual Vector3 DetermineSpellTarget(Vector3 start, int[] pattern, Vector3 end) {
        return start;
    }

    public void SpellInvokedDebugMessage(Vector3 p) {
        Debug.Log(gameObject.name + " invoked at " + p);
    }
}
