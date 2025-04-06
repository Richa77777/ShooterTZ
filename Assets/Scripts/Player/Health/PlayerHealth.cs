using System;
using UnityEngine;

[RequireComponent(typeof(HealthDisplay))]
public class PlayerHealth : MonoBehaviour, IDamagable
{
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Inventory _inventory;

    private HealthDisplay _healthDisplay;

    public int CurrentHealth { get; private set; }

    private void Awake()
    {
        CurrentHealth = _maxHealth;

        _healthDisplay = GetComponent<HealthDisplay>();
        _healthDisplay.UpdateHealthDisplay(CurrentHealth);
    }

    private void OnEnable()
    {
        _inventory.OnExtraItemUsed += AddHealthMedkit;
    }

    private void OnDisable()
    {
        _inventory.OnExtraItemUsed -= AddHealthMedkit;
    }

    public void AddHealthMedkit(ItemType itemType)
    {
        if (itemType == ItemType.Medkit)
        {
            AddHealth(100);
        }
    }

    public void AddHealth(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, _maxHealth);
        _healthDisplay.UpdateHealthDisplay(CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"Player has been damaged (-{damage})");

        CurrentHealth = Mathf.Clamp(CurrentHealth - damage, 0, _maxHealth);
        _healthDisplay.UpdateHealthDisplay(CurrentHealth);

        if (CurrentHealth <= 0)
            Die();
    }

    public void Die()
    {
        EventsHandler.Instance.OnPlayerDied?.Invoke();
        Debug.Log("Player Died");
    }
}