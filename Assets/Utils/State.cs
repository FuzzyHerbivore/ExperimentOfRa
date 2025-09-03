#nullable enable

using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract State? EnterState();
    public abstract State? ExitState();
    public abstract State? UpdateState();
}
