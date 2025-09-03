using System;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public UnityEvent<int> HealthUpdated;
    public UnityEvent HealthExhausted;

    [SerializeField] int maxHealth = 100;

    public int CurrentHealth { get; private set; }

    void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void RemoveHealth(int amount)
    {
        Func<int, int, int> subtraction = (x, y) => x - y;

        ModifyHealth(amount, subtraction);
    }

    public void AddHealth(int amount)
    {
        Func<int, int, int> addition = (x, y) => x + y;

        ModifyHealth(amount, addition);
    }

    void ModifyHealth(int amount, Func<int, int, int> operation)
    {
        int newHealth = operation(CurrentHealth, amount);

        if (newHealth <= 0)
        {
            HealthExhausted?.Invoke();
        }

        CurrentHealth = Mathf.Clamp(newHealth, 0, maxHealth);
        HealthUpdated?.Invoke(newHealth);
    }
}
