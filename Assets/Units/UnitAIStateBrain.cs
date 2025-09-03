#nullable enable

using UnityEngine;

public abstract class UnitAIStateBrain : MonoBehaviour
{
    public virtual bool ThinkEnterPreconditions()
    {
        return true;
    }

    public virtual State? ThinkEnterPreAct()
    {
        return null;
    }

    public virtual State? ThinkEnterPostAct()
    {
        return null;
    }

    public virtual State? ThinkExitPreAct()
    {
        return null;
    }

    public virtual State? ThinkExitPostAct()
    {
        return null;
    }

    public virtual State? ThinkUpdate()
    {
        return null;
    }

    public virtual void ActEnter() { }
    public virtual void ActExit() { }
    public virtual void ActUpdate() { }
}
