using UnityEngine;
using UnityEngine.Events;

public class SpellEventManager : MonoBehaviour
{
    public static SpellEventManager instance;

    public UnityEvent<Spell, Vector3, Vector3> onSpellCast = new UnityEvent<Spell, Vector3, Vector3>();
    public UnityEvent<int[]> onSpellFailed = new UnityEvent<int[]>();

    [SerializeField]
    private Spell[] spells;

    private void Awake()
    {
        instance = this;
    }

    void Start() {
        onSpellCast.AddListener(DoSpell);
    }

    public Spell DetermineSpell(int[] pattern) {
        foreach (Spell s in spells) {
            if (s.MatchPattern(pattern)) return s;
        }

        return null;
    }

    private void DoSpell(Spell spell, Vector3 origin, Vector3 target) {
        Spell instance = Instantiate(spell.gameObject, origin, new Quaternion()).GetComponent<Spell>();
        instance.onSpellInvoked.Invoke(target);
    }

    public void SpellFailPrintPattern(int[] pattern) {
        string s = "";
        foreach(int i in pattern) {
            s += " -> " + i;
        }

        Debug.Log(s);
    }
}
