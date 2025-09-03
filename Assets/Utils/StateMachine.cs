#nullable enable

using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] State? initialState;

    State? currentState;

    void Start()
    {
        if (initialState == null)
        {
            Debug.LogError($"No initial state set in {this}");
        }
        else
        {
            ChangeState(initialState);
        }
    }

    void FixedUpdate()
    {
        if (currentState == null) return;

        State? newState = currentState.UpdateState();

        if (newState != null)
        {
            ChangeState(newState);
        }
    }

    public void ChangeState(State newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        newState.EnterState();

        currentState = newState;
    }
}
