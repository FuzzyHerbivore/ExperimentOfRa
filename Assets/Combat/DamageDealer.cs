using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class DamageDealer : MonoBehaviour
    {
        [SerializeField] int attackStrength;
        [SerializeField] List<DamageTaker> excludeDamageTaker;

        readonly HashSet<DamageTaker> damageTakersInRange = new();

        void Awake()
        {
            if (!TryGetComponent(out Collider collider))
            {
                Debug.LogError($"DamageDealer component in {gameObject} does not provide a collider!");
                return;
            }

            if (!collider.isTrigger)
            {
                Debug.LogError($"Collider in DamageDealer component of {gameObject} is not configured as a trigger!");
                return;
            }
        }

        public void RegisterDamageTaker(DamageTaker damageTaker)
        {
            if (excludeDamageTaker.Contains(damageTaker)) return;

            damageTakersInRange.Add(damageTaker);
        }

        public void UnregisterDamageTaker(DamageTaker damageTaker)
        {
            damageTakersInRange.Remove(damageTaker);
        }

        public int GetDamageTakersInRangeCount()
        {
            return damageTakersInRange.Count;
        }

        public void DealDamage()
        {
            damageTakersInRange.RemoveWhere(damageTaker => damageTaker == null);

            foreach (DamageTaker damageTaker in damageTakersInRange)
            {
                damageTaker.TakeDamage(attackStrength);
            }
        }
    }
}
