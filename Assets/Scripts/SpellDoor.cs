using UnityEngine;

public class SpellDoor : MonoBehaviour
{
    [SerializeField] private Spell required;
    [SerializeField] private float distance = 3;

    private Transform diagram;

    void Start()
    {
        diagram = transform.GetChild(0);
        SpellEventManager.instance.onSpellCast.AddListener(CheckSpell);
    }

    private void Update()
    {
        diagram.localPosition = new Vector2(0, Mathf.Sin(Time.realtimeSinceStartup) * 0.02f);
    }

    private void CheckSpell(Spell spell, Vector3 _, Vector3 target)
    {
        if (Vector2.Distance(diagram.position, target) <= distance)
        {
            if (spell == required)
            {
                AudioManager.instance.doorUnlock.PlayAt(transform.position);
                ParticlePlayer.Activate(transform.GetChild(1).gameObject);
                Destroy(gameObject);
            }
            else AudioManager.instance.doorDeny.PlayAt(transform.position);
        }
    }
}
