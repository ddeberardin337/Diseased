using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorInteraction : MonoBehaviour, IUsable
{
    [SerializeField]
    private string interactionPrompt = "Press E to enter";
    public string InteractionPrompt => interactionPrompt;

    [SerializeField, HideInInspector]
    private string destinationScenePath;

    [SerializeField, HideInInspector]
    private string destinationSpawnPoint;

    public void Interact()
    {
        if (string.IsNullOrEmpty(destinationScenePath))
        {
            Debug.LogWarning("Door has no destination scene assigned!", this);
            return;
        }

        if (string.IsNullOrEmpty(destinationSpawnPoint))
        {
            Debug.LogWarning("Door has no destination spawn point assigned!", this);
            return;
        }

        SceneTransitionData.spawnPointName = destinationSpawnPoint;

        SceneManager.LoadScene(destinationScenePath);
    }
}