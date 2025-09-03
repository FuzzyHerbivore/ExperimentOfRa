#nullable enable

using UnityEngine;

public class GuardianIdleAIStateBrain : UnitAIStateBrain
{
    [SerializeField] UnitAIState? takeDamageState;
    [SerializeField] UnitAIState? dieState;

    void Awake()
    {
        if (takeDamageState == null)
        {
            Debug.LogError($"{this} has no Take Damage State set!");
        }

        if (dieState == null)
        {
            Debug.LogError($"{this} has no Die State set!");
        }
    }

    public override State? ThinkUpdate()
    {
        if (takeDamageState == null) return null;
        if (dieState == null) return null;

        UnitAIStateBrain? stateBrain;

        stateBrain = dieState.GetAIStateBrain();
        if (stateBrain is GuardianDieAIStateBrain guardianDieStateBrain)
        {
            if (guardianDieStateBrain.ThinkEnterPreconditions())
            {
                return dieState;
            }
        }

        stateBrain = takeDamageState.GetAIStateBrain();
        if (stateBrain is GuardianTakeDamageAIStateBrain takeDamageStateBrain)
        {
            if (takeDamageStateBrain.ThinkEnterPreconditions())
            {
                return takeDamageState;
            }
        }

        return null;
    }
}
