using UnityEngine;

public interface IDamagable
{
    int CurrentHealth { get; }
    void TakeDamage(int damage);
    void Die();
}
