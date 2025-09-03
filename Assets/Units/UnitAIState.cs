#nullable enable

using UnityEngine;

public class UnitAIState : State
{
    [SerializeField] UnitAIStateBrain? brain;

    public override State? EnterState()
    {
        if (brain == null) return null;

        State? interceptingState = brain.ThinkEnterPreAct();
        if (interceptingState != null) return interceptingState;

        brain.ActEnter();

        return brain.ThinkEnterPostAct();
    }

    public override State? ExitState()
    {
        if (brain == null) return null;

        State? interceptingState = brain.ThinkExitPreAct();
        if (interceptingState != null) return interceptingState;

        brain.ActExit();

        return brain.ThinkExitPostAct();
    }

    public override State? UpdateState()
    {
        if (brain == null) return null;

        State? interceptingState = brain.ThinkUpdate();
        if (interceptingState != null) return interceptingState;

        brain.ActUpdate();

        return null;
    }

    public UnitAIStateBrain? GetAIStateBrain()
    {
        return brain;
    }
}
