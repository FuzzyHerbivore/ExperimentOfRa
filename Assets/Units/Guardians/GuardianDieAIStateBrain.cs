#nullable enable

using UnityEngine;

public class GuardianDieAIStateBrain : UnitAIStateBrain
{
    [SerializeField] Animator? animator;
    [SerializeField] Health? health;

    bool didJustDie = false;

    void Awake()
    {
        if (animator == null)
        {
            Debug.LogError($"{this} has no Animator set!");
        }

        if (health == null)
        {
            Debug.LogError($"{this} has no Health set!");
        }
    }

    void Start()
    {
        if (health == null) return;

        health.HealthExhausted.AddListener(OnHealthExhausted);
    }

    void OnDestroy()
    {
        if (health == null) return;

        health.HealthExhausted.RemoveListener(OnHealthExhausted);
    }

    public override bool ThinkEnterPreconditions()
    {
        if (didJustDie) return true;

        return false;
    }

    public override void ActEnter()
    {
        if (animator == null) return;

        animator.SetTrigger("die");
    }

    public void OnHealthExhausted()
    {
        didJustDie = true;
    }

    public void Despawn()
    {
        Destroy(gameObject);
    }
}
