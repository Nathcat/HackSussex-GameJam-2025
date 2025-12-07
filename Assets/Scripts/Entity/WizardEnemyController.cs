using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class WizardEnemyController : EnemyController {

    [SerializeField]
    private Spell spell;

    protected override void AttackEntity(Entity player) {
        if (!attackOnCooldown) {
            base.StartAttackAnimation();
            
            SpellEventManager.instance.onSpellCast.Invoke(spell, transform.position, player.transform.position);

            attackOnCooldown = true;

            this.RunAfter(attackCooldown, () => { attackOnCooldown = false; });
        }
    }
}
