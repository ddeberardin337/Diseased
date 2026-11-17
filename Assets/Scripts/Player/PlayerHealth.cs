
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;

    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public bool IsDead => currentHealth <= 0f;

    public void Heal(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        currentHealth = Mathf.Clamp(currentHealth + amount, 0f, maxHealth);

        Debug.Log("Health: " + currentHealth + "/" + maxHealth);
    }

    public void TakeDamage(float amount)
    {
        if (amount <= 0f || IsDead)
            return;

        currentHealth = Mathf.Clamp(currentHealth - amount, 0f, maxHealth);

        Debug.Log("Health: " + currentHealth + "/" + maxHealth);
    }
}
