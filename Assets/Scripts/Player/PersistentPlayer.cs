using UnityEngine;

public class PersistentPlayer : MonoBehaviour
{
    private static PersistentPlayer instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static void DestroyPersistentPlayer()
    {
        if (instance == null)
            return;

        GameObject player = instance.gameObject;

        instance = null;

        player.SetActive(false);
        Destroy(player);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}