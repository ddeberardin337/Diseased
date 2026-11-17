
using UnityEngine;

public class PotionPickup : MonoBehaviour, IUsable
{
    [SerializeField]
    private string interactionPrompt = "Press E to drink";

    public string InteractionPrompt => interactionPrompt;

    private bool consumed = false;

    public void Interact()
    {
        if (consumed)
            return;

        V2PlayerMovement playerMovement =
            FindFirstObjectByType<V2PlayerMovement>();

        PotionScreenEffect screenEffect =
            FindFirstObjectByType<PotionScreenEffect>();

        if (playerMovement == null || screenEffect == null)
        {
            Debug.LogError("Player movement or screen effect not found!");
            return;
        }

        consumed = true;

        // Freeze player movement
        playerMovement.CanMove = false;

        SceneTransitionData.spawnPointName = "DistortedRealitySpawn";

        // Start the visual and scene transition sequence
        screenEffect.StartPotionTransition();

        // Remove the potion
        Destroy(gameObject);
    }
}
