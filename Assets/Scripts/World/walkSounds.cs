using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class walkSounds : MonoBehaviour
{
    [Header("Audio Clips")]
    public AudioClip waGroupSound;
    public AudioClip sdGroupSound;

    [Header("Settings")]
    [Range(0f, 1f)]
    public float volume = 1.0f;
    public float stepInterval = 0.35f; 
    private AudioSource audioSource;
    private float stepTimer;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    void Update()
    {
        bool pressingWA = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A);
        bool pressingSD = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (pressingWA || pressingSD)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                if (pressingWA) PlayClip(waGroupSound);
                else if (pressingSD) PlayClip(sdGroupSound);

                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; 
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.pitch = Random.Range(0.92f, 1.08f);
            audioSource.PlayOneShot(clip, volume);
        }
    }
}