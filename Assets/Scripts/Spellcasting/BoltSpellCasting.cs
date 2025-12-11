using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class BoltSpellCasting : Spell
{
    public override bool MatchPattern(int[] pattern) {
        return pattern.Length == 1;
    }

    public override Vector3 DetermineSpellTarget(Vector3 start, int[] pattern, Vector3 end) {
        Vector3 direction = Vector3.Normalize(end - start);
        float distance = (start - GameManager.instance.player.transform.position).magnitude;
        return GameManager.instance.player.transform.position + (direction * distance);
    }
}
