using UnityEngine;

public class SpellDoor : SpellHint
{
    [SerializeField] private Spell required;
    [SerializeField] private Sprite error;

    private SpriteRenderer rune;
    private float timer = 0;
    private Sprite initial;

    void Start()
    {
        SpellEventManager.instance.onSpellCast.AddListener(CheckSpell);
        SpellEventManager.instance.onSpellFailed.AddListener((_) => { if (inRange) Failed(); });
        rune = transform.GetChild(0).GetComponent<SpriteRenderer>();
        initial = rune.sprite;
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
            }
            else Failed();
        }
    }

    private void Failed()
    {
        AudioManager.instance.doorDeny.PlayAt(transform.position);
        rune.sprite = error;
        timer = 2;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0) rune.sprite = initial;
        }
    }
}
