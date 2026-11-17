using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    private static PersistentUI instance;

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

    public static void DestroyPersistentUI()
    {
        if (instance == null)
            return;

        GameObject ui = instance.gameObject;

        instance = null;

        ui.SetActive(false);
        Destroy(ui);
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}