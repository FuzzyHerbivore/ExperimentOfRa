#nullable enable

using Assets.Combat;
using UnityEngine;

public class EnemyAttackAIStateBrain : UnitAIStateBrain
{
    [SerializeField] Animator? animator;
    [SerializeField] DamageDealer? damageDealer;
    [SerializeField] UnitAIState? idleState;

    void Awake()
    {
        if (animator == null)
        {
            Debug.LogError($"{this} has no Animator set!");
        }

        if (idleState == null)
        {
            Debug.LogError($"{this} has no Idle State set!");
        }

        if (damageDealer == null)
        {
            Debug.LogError($"{this} does not have a DamageDealer set!");
        }
    }

    public override bool ThinkEnterPreconditions()
    {
        if (damageDealer == null) return false;

        if (damageDealer.GetDamageTakersInRangeCount() > 0)
        {
            return true;
        }

        return false;
    }

    public override State? ThinkUpdate()
    {
        if (damageDealer == null) return null;

        if (damageDealer.GetDamageTakersInRangeCount() <= 0)
        {
            return idleState;
        }

        return null;
    }

    public override void ActEnter()
    {
        if (animator == null) return;

        animator.SetBool("isAttacking", true);
    }

    public override void ActExit()
    {
        if (animator == null) return;

        animator.SetBool("isAttacking", false);
    }

    public void DealDamage()
    {
        if (damageDealer == null) return;

        damageDealer.DealDamage();
    }
}
