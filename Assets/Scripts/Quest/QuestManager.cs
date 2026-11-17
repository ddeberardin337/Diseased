
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance { get; private set; }

    public enum EndingChoice
    {
        None,
        TakeCure,
        DestroyCure
    }

    [Header("Quest")]
    [SerializeField] private QuestData questData;

    private int currentItemIndex = 0;

    public int CurrentItemIndex => currentItemIndex;

    public bool IsQuestComplete =>
        currentItemIndex >= questData.requiredItems.Length;

    private bool introductionCompleted = false;

    public bool IntroductionCompleted => introductionCompleted;

    private bool potionSpawned = false;

    public bool PotionSpawned => potionSpawned;

    [Header("Ending Scenes")]
    [SerializeField] private string goodEndingScene;
    [SerializeField] private string badEndingScene;

    private EndingChoice endingChoice = EndingChoice.None;

    public EndingChoice CurrentEndingChoice => endingChoice;

    public ItemData CurrentRequiredItem
    {
        get
        {
            if (IsQuestComplete)
                return null;

            return questData.requiredItems[currentItemIndex];
        }
    }

    private void Awake()
    {
        if (questData == null ||
            questData.requiredItems == null ||
            questData.requiredItems.Length != 3)
        {
            Debug.LogError("QuestData must contain exactly three items!", this);
            enabled = false;
            return;
        }

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CompleteIntroduction()
    {
        introductionCompleted = true;
    }

    public bool TryMarkPotionSpawned()
    {
        if (potionSpawned)
            return false;

        potionSpawned = true;
        return true;
    }

    public bool CanDeliverItem(ItemData item)
    {
        return !IsQuestComplete &&
               item == CurrentRequiredItem;
    }

    public bool DeliverItem(ItemData item)
    {
        if (!CanDeliverItem(item))
            return false;

        currentItemIndex++;

        Debug.Log("Quest progress: " +
                  currentItemIndex + "/" +
                  questData.requiredItems.Length);

        QuestItemPickup[] pickups =
            FindObjectsByType<QuestItemPickup>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (QuestItemPickup pickup in pickups)
        {
            pickup.RefreshAvailability();
        }

        if (IsQuestComplete)
            Debug.Log("MAIN QUEST COMPLETE!");

        return true;
    }

    public bool IsItemUnlocked(ItemData item)
    {
        if (IsQuestComplete || item == null)
            return false;

        return CurrentRequiredItem == item;
    }

    // Player accepts the cure: BAD ENDING.
    public void TakeCure()
    {
        LoadEnding(EndingChoice.TakeCure);
    }

    // Player destroys the cure: GOOD ENDING.
    public void DestroyCure()
    {
        LoadEnding(EndingChoice.DestroyCure);
    }

    private void LoadEnding(EndingChoice choice)
    {
        if (!IsQuestComplete)
        {
            Debug.LogWarning("The ending is not unlocked yet!");
            return;
        }

        if (endingChoice != EndingChoice.None)
        {
            Debug.LogWarning("An ending has already been selected!");
            return;
        }

        endingChoice = choice;

        string sceneToLoad;

        switch (choice)
        {
            case EndingChoice.TakeCure:
                sceneToLoad = badEndingScene;
                Debug.Log("Player accepted the cure. BAD ENDING.");
                break;

            case EndingChoice.DestroyCure:
                sceneToLoad = goodEndingScene;
                Debug.Log("Player destroyed the cure. GOOD ENDING.");
                break;

            default:
                Debug.LogWarning("Invalid ending choice!");
                return;
        }

        if (string.IsNullOrEmpty(sceneToLoad))
        {
            Debug.LogError("Ending scene has not been assigned!");
            return;
        }

        PersistentPlayer.DestroyPersistentPlayer();
        PersistentUI.DestroyPersistentUI();

        SceneManager.LoadScene(sceneToLoad);
    }
}
