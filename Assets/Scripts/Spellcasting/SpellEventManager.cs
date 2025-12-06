using UnityEngine;
using UnityEngine.Events;

public class SpellEventManager : MonoBehaviour
{
    public static SpellEventManager instance;

    public UnityEvent<Spell, Vector3> onSpellCast = new UnityEvent<Spell, Vector3>();
    public UnityEvent<int[]> onSpellFailed = new UnityEvent<int[]>();
    public Vector3 spellOrigin;

    [SerializeField]
    private Spell[] spells;

    void Start() {
        instance = this;

        onSpellCast.AddListener(DoSpell);
    }

    public Spell DetermineSpell(int[] pattern) {
        foreach (Spell s in spells) {
            if (s.MatchPattern(pattern)) return s;
        }

        return null;
    }

    private void DoSpell(Spell spell, Vector3 position) {
        Spell instance = Instantiate(spell.gameObject, spellOrigin, new Quaternion()).GetComponent<Spell>();
        instance.onSpellInvoked.Invoke(position);
    }

    public void SpellFailPrintPattern(int[] pattern) {
        string s = "";
        foreach(int i in pattern) {
            s += " -> " + i;
        }

        Debug.Log(s);
    }
}
