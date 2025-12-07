using UnityEngine;

public class BossEnemyController : EnemyController
{
    [SerializeField]
    private Spell spell;

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
}
