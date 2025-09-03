#nullable enable

using UnityEngine;

public class GuardianTakeDamageAIStateBrain : UnitAIStateBrain
{
    [SerializeField] Animator? animator;
    [SerializeField] Health? health;
    [SerializeField] UnitAIState? idleState;

    bool didJustTakeDamage = false;

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

        if (health == null)
        {
            Debug.LogError($"{this} does not have a Health set!");
        }
    }

    void Start()
    {
        if (health == null) return;

        health.HealthUpdated.AddListener(OnHealthUpdated);
    }

    void OnDestroy()
    {
        if (health == null) return;

        health.HealthUpdated.RemoveListener(OnHealthUpdated);
    }

    public override bool ThinkEnterPreconditions()
    {
        if (didJustTakeDamage) return true;

        return false;
    }

    public override State? ThinkUpdate()
    {
        if (didJustTakeDamage)
        {
            didJustTakeDamage = false;
            return idleState;
        }

        return null;
    }

    public override void ActEnter()
    {
        if (animator == null) return;

        animator.SetTrigger("takeDamage");
    }

    void OnHealthUpdated(int newHealth)
    {
        Debug.Log($"{gameObject} now has {newHealth} health left!");

        didJustTakeDamage = true;
    }
}
