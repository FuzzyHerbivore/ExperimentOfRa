#nullable enable

using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;
using UnityEngine.AI;

public class EnemyApproachAIStateBrain : UnitAIStateBrain
{
    [SerializeField] Animator? animator;
    [SerializeField] NavMeshAgent? navMeshAgent;
    [SerializeField] float recalculatePathCooldown = 0.5f;
    [SerializeField] DamageDealer? damageDealer;
    [SerializeField] CombatantDetector? combatantDetector;
    [SerializeField] UnitAIState? idleState;

    Transform? targetTransform;
    float remainingRecalculatePathCooldown;

    void Awake()
    {
        if (animator == null)
        {
            Debug.LogError($"{this} has no Animator set!");
        }

        if (navMeshAgent == null)
        {
            Debug.LogError($"{this} has no NavMeshAgent set!");
        }

        if (idleState == null)
        {
            Debug.LogError($"{this} does not have an Idle State set!");
        }

        if (damageDealer == null)
        {
            Debug.LogError($"{this} does not have a DamageDealer set!");
        }

        if (combatantDetector == null)
        {
            Debug.LogError($"{this} does not have a CombatantDetector set!");
        }
    }

    public override bool ThinkEnterPreconditions()
    {
        if (navMeshAgent == null) return false;
        if (damageDealer == null) return false;
        if (combatantDetector == null) return false;

        DamageTaker? targetedCombatant = GetClosestCombatant(combatantDetector.GetDetectedCombatants());

        if (targetedCombatant != null)
        {
            targetTransform = targetedCombatant.transform;
            navMeshAgent.destination = targetTransform.position;

            if (!HasReachedDestination())
            {
                return true;
            }
        }

        return false;
    }

    public override State? ThinkEnterPreAct()
    {
        remainingRecalculatePathCooldown = recalculatePathCooldown;

        return null;
    }

    public override void ActExit()
    {
        if (animator == null) return;

        animator.SetFloat("speed", 0);
    }

    public override State? ThinkUpdate()
    {
        if (navMeshAgent == null) return null;
        if (targetTransform == null) return null;
        if (combatantDetector == null) return null;

        remainingRecalculatePathCooldown -= Time.deltaTime;

        if (remainingRecalculatePathCooldown <= 0)
        {
            remainingRecalculatePathCooldown += recalculatePathCooldown;

            DamageTaker? targetedCombatant = GetClosestCombatant(combatantDetector.GetDetectedCombatants());
            if (targetedCombatant != null)
            {
                targetTransform = targetedCombatant.transform;
            }
            navMeshAgent.destination = targetTransform.position;
        }

        if (HasReachedDestination())
        {
            return idleState;
        }

        return null;
    }

    public override void ActUpdate()
    {
        if (navMeshAgent == null) return;
        if (animator == null) return;

        animator.SetFloat("speed", navMeshAgent.velocity.magnitude);
    }

    DamageTaker? GetClosestCombatant(HashSet<DamageTaker> combatants)
    {
        if (damageDealer == null) return null;

        float smallestSquaredDistanceToCombatant = float.PositiveInfinity;
        DamageTaker? targetedCombatant = null;

        foreach (DamageTaker combatant in combatants)
        {
            float squaredDistanceToCombatant = (combatant.transform.position - damageDealer.transform.position).sqrMagnitude;

            if (squaredDistanceToCombatant < smallestSquaredDistanceToCombatant)
            {
                smallestSquaredDistanceToCombatant = squaredDistanceToCombatant;
                targetedCombatant = combatant;
            }
        }

        return targetedCombatant;
    }

    bool HasReachedDestination()
    {
        if (navMeshAgent == null) return false;
        if (navMeshAgent.pathPending) return false;
        if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance) return false;
        if (navMeshAgent.hasPath && navMeshAgent.velocity.sqrMagnitude != 0) return false;

        return true;
    }
}
