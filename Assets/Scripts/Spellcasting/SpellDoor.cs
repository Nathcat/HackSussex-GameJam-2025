using UnityEngine;

public class SpellDoor : SpellHint
{
    [SerializeField] private Spell required;
    [SerializeField] private Sprite error;

    private SpriteRenderer rune;

    void Start()
    {
        SpellEventManager.instance.onSpellCast.AddListener(CheckSpell);
        SpellEventManager.instance.onSpellFailed.AddListener((_) => { if (inRange) Failed(); });
        rune = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void CheckSpell(Spell spell, Vector3 _, Vector3 target)
    {
        if (inRange)
        {
            if (spell == required)
            {
                AudioManager.instance.doorUnlock.PlayAt(transform.position);
                ParticlePlayer.Activate(transform.GetChild(1).gameObject);
                Destroy(gameObject);
                
                GameManager.instance.player.invulnerable = true;
                GameManager.instance.RunAfter(1, () => { GameManager.instance.player.invulnerable = false; });
            }
            else Failed();
        }
    }

    private void Failed()
    {
        AudioManager.instance.doorDeny.PlayAt(transform.position);
        Sprite old = rune.sprite;
        rune.sprite = error;
        this.RunAfter(2, () => { rune.sprite = old; });
    }
}
