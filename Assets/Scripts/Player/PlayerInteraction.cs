
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private List<IUsable> nearbyInteractables = new List<IUsable>();

    private IUsable currentInteractable;
    private InteractionPromptUI promptUI;

    [Header("Pickup Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] pickupSounds;
    [SerializeField] private AudioClip[] craftingSounds;

    private void Start()
    {
        promptUI = FindFirstObjectByType<InteractionPromptUI>();

        if (promptUI == null)
        {
            Debug.LogWarning("Interaction Prompt UI not found!");
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        UpdateCurrentInteractable();

        if (currentInteractable != null &&
            Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            // Play a sound only when picking up an item
            if (currentInteractable is ItemPickup ||
                currentInteractable is QuestItemPickup)
            {
                PlayPickupSound();
            }

            currentInteractable.Interact();
        }
    }

    private void PlayPickupSound()
    {
        if (audioSource == null || pickupSounds == null || pickupSounds.Length == 0)
            return;

        AudioClip randomSound = pickupSounds[
            Random.Range(0, pickupSounds.Length)
        ];

        if (randomSound != null)
        {
            audioSource.PlayOneShot(randomSound);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IUsable usable = other.GetComponentInParent<IUsable>();

        if (usable != null && !nearbyInteractables.Contains(usable))
        {
            nearbyInteractables.Add(usable);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IUsable usable = other.GetComponentInParent<IUsable>();

        if (usable != null)
        {
            nearbyInteractables.Remove(usable);
        }
    }

    private void UpdateCurrentInteractable()
    {
        IUsable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (IUsable usable in nearbyInteractables)
        {
            if (usable is not MonoBehaviour behaviour || behaviour == null)
                continue;

            float distance = Vector2.Distance(
                transform.position,
                behaviour.transform.position
            );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestInteractable = usable;
            }
        }

        if (currentInteractable == closestInteractable)
            return;

        currentInteractable = closestInteractable;

        if (currentInteractable != null)
        {
            promptUI?.ShowPrompt(currentInteractable.InteractionPrompt);
        }
        else
        {
            promptUI?.HidePrompt();
        }
    }


    public void PlayCraftingSound()
    {
        PlayRandomSound(craftingSounds);
    }

    private void PlayRandomSound(AudioClip[] sounds)
    {
        if (audioSource == null || sounds == null || sounds.Length == 0)
            return;

        AudioClip randomSound = sounds[Random.Range(0, sounds.Length)];

        if (randomSound != null)
        {
            audioSource.PlayOneShot(randomSound);
        }
    }
}
