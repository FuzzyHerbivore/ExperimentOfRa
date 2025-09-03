using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public abstract class CombatantDetector : MonoBehaviour
    {
        [SerializeField] protected string[] detectedTags;

        public abstract HashSet<DamageTaker> GetDetectedCombatants();
    }
}
