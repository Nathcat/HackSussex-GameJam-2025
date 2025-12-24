using System.Runtime.CompilerServices;
using UnityEngine;

public class BossEnemyController : EnemyController
{
    [SerializeField] private Spell spell;
    [SerializeField] private SpriteRenderer crown;

    protected override void AttackEntity(Entity player)
    {
        if (!attackOnCooldown)
        {
            base.StartAttackAnimation();

            SpellEventManager.instance.onSpellCast.Invoke(spell, transform.position, transform.position);

            attackOnCooldown = true;

            this.RunAfter(attackCooldown, () => { attackOnCooldown = false; });
        }
    }

    protected override void Start()
    {
        base.Start();
        base.onDeath.AddListener(OnDeath);
    }

    protected override void Update()
    {
        base.Update();
        crown.flipX = sprite.flipX;
    }

    private void OnDeath()
    {
        crown.transform.SetParent(null);
        crown.GetComponent<Rigidbody2D>().simulated = true;
    }
}
