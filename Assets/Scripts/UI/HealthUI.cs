using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private Image healthBarFill;

    private PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component not found in the scene.");
            return;
        }
        
    }

    
    private void Update()
    {
        if (playerHealth == null || healthBarFill == null)
        {
            Debug.LogWarning("PlayerHealth or healthBarFill is not assigned.");
            return;
        }

        float healthPercentage = playerHealth.CurrentHealth / playerHealth.MaxHealth;

        healthBarFill.fillAmount = Mathf.Clamp01(healthPercentage);
    }
}
