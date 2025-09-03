using UnityEngine;

namespace Assets.Combat
{
    [RequireComponent(typeof(Health))]
    public class DamageTaker : MonoBehaviour
    {
        [SerializeField] int defenseStrength;

        Health health;

        void Awake()
        {
            if (!TryGetComponent(out Collider collider))
            {
                Debug.Log($"DamageTaker component in {gameObject} does not provide a Collider component!");
                return;
            }

            if (!collider.isTrigger)
            {
                Debug.Log($"Collider in DamageTaker component of {gameObject} is not configured as a trigger!");
                return;
            }
        }

        void Start()
        {
            health = GetComponent<Health>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out DamageDealer damageDealer)) return;

            damageDealer.RegisterDamageTaker(this);
        }

        void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out DamageDealer damageDealer)) return;

            damageDealer.UnregisterDamageTaker(this);
        }

        public void TakeDamage(int attackStrength)
        {
            int damage = attackStrength - defenseStrength;
            health.RemoveHealth(damage);
        }
    }
}
