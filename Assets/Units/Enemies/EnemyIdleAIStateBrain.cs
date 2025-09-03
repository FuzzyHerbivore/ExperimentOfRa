#nullable enable

using UnityEngine;

public class EnemyIdleAIStateBrain : UnitAIStateBrain
{
    [SerializeField] UnitAIState? approachState;
    [SerializeField] UnitAIState? attackState;

    void Awake()
    {
        if (approachState == null)
        {
            Debug.LogError($"{this} has no Approach State set!");
        }

        if (attackState == null)
        {
            Debug.LogError($"{this} has no Attack State set!");
        }
    }

    public override State? ThinkUpdate()
    {
        if (approachState == null) return null;
        if (attackState == null) return null;

        UnitAIStateBrain? stateBrain;

        stateBrain = attackState.GetAIStateBrain();
        if (stateBrain is EnemyAttackAIStateBrain attackStateBrain)
        {
            if (attackStateBrain.ThinkEnterPreconditions())
            {
                return attackState;
            }
        }

        stateBrain = approachState.GetAIStateBrain();
        if (stateBrain is EnemyApproachAIStateBrain approachStateBrain)
        {
            if (approachStateBrain.ThinkEnterPreconditions())
            {
                return approachState;
            }
        }

        return null;
    }
}
