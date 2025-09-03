#nullable enable

using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class CombatantDetectorByReach : CombatantDetector
    {
        HashSet<DamageTaker> combatantsInReach = new();

        void Awake()
        {
            if (!TryGetComponent(out Collider collider)) Debug.LogError($"{gameObject} does not provide a Collider component!");

            if (!collider.isTrigger)
            {
                Debug.Log($"Collider in CombatantDetectorByReach component of {gameObject} is not configured as a trigger!");
                return;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            DamageTaker? combatant = GetValidCombatant(other);

            if (combatant == null) return;

            combatantsInReach.Add(combatant);
        }

        void OnTriggerExit(Collider other)
        {
            DamageTaker? combatant = GetValidCombatant(other);

            if (combatant == null) return;

            combatantsInReach.Remove(combatant);
        }

        DamageTaker? GetValidCombatant(Collider collider)
        {
            if (!collider.TryGetComponent(out DamageTaker combatant)) return null;
            foreach (string tag in detectedTags)
            {
                if (!combatant.CompareTag(tag)) return null;
            }

            return combatant;
        }

        public override HashSet<DamageTaker> GetDetectedCombatants()
        {
            combatantsInReach.RemoveWhere(damageTaker => damageTaker == null);
            return combatantsInReach;
        }
    }
}
