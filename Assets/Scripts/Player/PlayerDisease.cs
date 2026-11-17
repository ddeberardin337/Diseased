
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerDisease : MonoBehaviour
{
    [Header("Disease Settings")]
    [SerializeField] private bool startDiseased = true;
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float damageInterval = 10f;

    [Header("Wheezing Settings")]
    [SerializeField] private AudioClip[] wheezingSounds;
    [SerializeField] private float minimumWheezeInterval = 35f;
    [SerializeField] private float maximumWheezeInterval = 75f;
    [SerializeField, Range(0f, 1f)] private float wheezeVolume = 0.65f;
    [SerializeField] private Vector2 pitchRange = new Vector2(0.9f, 1.1f);

    [SerializeField] private float diseaseDamageMultiplier = 1f;
    [SerializeField] private float damageReductionTimer;

    private PlayerHealth playerHealth;
    private AudioSource audioSource;

    private bool diseaseActive;
    private bool diseasePaused;

    private float damageTimer;
    private float wheezeTimer;
    private float suppressionTimer;

    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
        audioSource = GetComponent<AudioSource>();

        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth component is missing!", this);
            enabled = false;
            return;
        }

        diseaseActive = startDiseased;

        ResetTimers();
    }


    private void Update()
    {
        // Update temporary damage reduction.
        if (damageReductionTimer > 0f)
        {
            damageReductionTimer -= Time.deltaTime;

            if (damageReductionTimer <= 0f)
                diseaseDamageMultiplier = 1f;
        }

        if (!diseaseActive || diseasePaused || playerHealth.IsDead)
            return;

        // Handle temporary disease suppression.
        if (suppressionTimer > 0f)
        {
            suppressionTimer -= Time.deltaTime;
            return;
        }

        // Handle gradual health deterioration.
        damageTimer -= Time.deltaTime;

        if (damageTimer <= 0f)
        {
            float finalDamage = damageAmount * diseaseDamageMultiplier;
            playerHealth.TakeDamage(finalDamage);
            damageTimer = damageInterval;
        }

        // Handle randomized wheezing.
        wheezeTimer -= Time.deltaTime;

        if (wheezeTimer <= 0f)
        {
            PlayWheeze();
            ResetWheezeTimer();
        }
    }


    private void PlayWheeze()
    {
        if (wheezingSounds == null || wheezingSounds.Length == 0)
            return;

        AudioClip selectedSound =
            wheezingSounds[Random.Range(0, wheezingSounds.Length)];

        audioSource.pitch =
            Random.Range(pitchRange.x, pitchRange.y);

        audioSource.PlayOneShot(selectedSound, wheezeVolume);
    }

    private void ResetTimers()
    {
        damageTimer = damageInterval;
        ResetWheezeTimer();
    }

    private void ResetWheezeTimer()
    {
        wheezeTimer = Random.Range(
            minimumWheezeInterval,
            maximumWheezeInterval
        );
    }

    // Temporarily freezes all disease activity.
    public void PauseDisease()
    {
        diseasePaused = true;
    }

    // Allows disease activity to resume.
    public void ResumeDisease()
    {
        diseasePaused = false;
    }

    // Temporarily disables disease effects for a duration.
    public void SuppressDisease(float duration)
    {
        suppressionTimer = Mathf.Max(suppressionTimer, duration);
    }

    // Completely removes the disease.
    public void CureDisease()
    {
        diseaseActive = false;
        diseasePaused = false;
        suppressionTimer = 0f;

        audioSource.Stop();
    }

    // Applies the disease again.
    public void ApplyDisease()
    {
        diseaseActive = true;
        diseasePaused = false;
        suppressionTimer = 0f;

        ResetTimers();
    }

    public void ReduceDiseaseDamage(float multiplier, float duration)
    {
        diseaseDamageMultiplier = Mathf.Clamp01(multiplier);
        damageReductionTimer = Mathf.Max(damageReductionTimer, duration);
    }
}
