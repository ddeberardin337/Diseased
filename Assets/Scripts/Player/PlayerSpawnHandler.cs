
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnHandler : MonoBehaviour
{
    private V2PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<V2PlayerMovement>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SCENE LOADED: " + scene.name);

        string spawnName = SceneTransitionData.spawnPointName;

        Debug.Log("Spawn point requested: " + spawnName);

        if (string.IsNullOrEmpty(spawnName))
        {
            Debug.LogWarning("No spawn point name provided!");
            return;
        }

        GameObject spawnPoint = GameObject.Find(spawnName);

        if (spawnPoint == null)
        {
            Debug.LogWarning("Spawn point not found: " + spawnName);
            SceneTransitionData.spawnPointName = null;
            return;
        }

        transform.position = spawnPoint.transform.position;

        Debug.Log("Player positioned at spawn point.");

        playerMovement = GetComponent<V2PlayerMovement>();

        if (playerMovement == null)
        {
            Debug.LogError("V2PlayerMovement NOT FOUND on player!");
            return;
        }

        Debug.Log("Movement before restoration: " + playerMovement.CanMove);

        playerMovement.CanMove = true;

        Debug.Log("Movement after restoration: " + playerMovement.CanMove);

        SceneTransitionData.spawnPointName = null;
    }

}
