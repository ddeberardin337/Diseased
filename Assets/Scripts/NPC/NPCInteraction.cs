
using UnityEngine;

public class NPCInteraction : MonoBehaviour, IUsable
{
    [Header("NPC Information")]
    [SerializeField] private string npcName = "The Physician";

    [SerializeField]
    private string interactionPrompt = "Press E to talk";

    [Header("Potion Spawn")]
    [SerializeField] private GameObject potionPrefab;
    [SerializeField] private Transform potionSpawnPoint;

    public string NPCName => npcName;
    public string InteractionPrompt => interactionPrompt;

    private string[] dialogueLines;
    private int dialogueIndex;

    private enum DialogueAction
    {
        None,
        CompleteIntroduction,
        DeliverItem,
        SpawnPotion,
        ChooseEnding
    }

    private DialogueAction currentAction;
    private ItemData itemToDeliver;

    public void Interact()
    {
        NPCDialogueUI dialogueUI =
            FindFirstObjectByType<NPCDialogueUI>();

        if (dialogueUI == null)
        {
            Debug.LogError("NPCDialogueUI could not be found!");
            return;
        }

        dialogueUI.OpenDialogue(this);
    }

    public void BeginDialogue()
    {
        QuestManager quest = QuestManager.Instance;

        if (quest == null)
            return;

        if (!quest.IntroductionCompleted)
        {
            StartIntroduction();
            return;
        }

        if (quest.IsQuestComplete)
        {
            StartEnding();
            return;
        }

        ItemData requiredItem = quest.CurrentRequiredItem;
        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory != null && inventory.HasItem(requiredItem))
        {
            StartItemDelivery(requiredItem);
        }
        else
        {
            StartItemRequest(requiredItem);
        }
    }

    private void SetDialogue(string[] lines, DialogueAction action)
    {
        dialogueLines = lines;
        dialogueIndex = 0;
        currentAction = action;
        itemToDeliver = null;
    }

    private void StartIntroduction()
    {
        SetDialogue(new string[]
        {
            "Ah. You've come too. Long is the night when the sickness came over you.",

            "Who am I? A friend, a caretaker, maybe even a physician. I have been studying this affliction for some time.",

            "Your ailment is a vile wrought of legends. Something that will eat your body from the inside out. It is not a disease, but a curse.",

            "Those afflicted rarely live long enough to understand what has happened to them.",

            "I have come on account of this sickness revealing itself. I can help, but you need to follow my instructions. Your life is on the line.",

            "There are three items I need to make your, er, cure. The relics you must find to be saved from the darkness that fills you.",

            "Then I fear your death is inevitable. I would advise against wasting what little time you have."
        }, DialogueAction.CompleteIntroduction);
    }

    private void StartItemRequest(ItemData item)
    {
        string[] lines;

        switch (QuestManager.Instance.CurrentItemIndex)
        {
            case 0:
                lines = new string[]
                {
                    "The first thing I need is an ancient scroll. It holds the, errr, recipe for this cure.",

                    "It's written in an ancient script, I doubt you would be able to translate it. I have studied the language for quite some time.",

                    "I suspect those that call themselves priests and enlightened ones have kept it in their chambers.",

                    "Your eyes look heavy, you don't have long. Bring it to me."
                };
                break;

            case 1:
                lines = new string[]
                {
                    "The scroll has given us a direction. But we are not finished.",

                    "Our next requirement is a gemstone. It is said to hold something that cannot be seen, but can certainly be felt.",

                    "It is a strange object, but the cure requires it. Yes, 'to pour the contents over the stone...'",

                    "Find the gemstone. Bring it back to me. I see the disease is only getting worse... Make haste. Who knows how much time you have."
                };
                break;

            default:
                lines = new string[]
                {
                    "Only one thing remains.",

                    "A serpent, preserved within a jar. An unusual request, I know, but its significance cannot be overstated.",

                    "It's venom is rumoured to bring about, errr, well, a cure. It will expell the disease from your body.",

                    "Find it. Once you return, we can finally begin."
                };
                break;
        }

        SetDialogue(lines, DialogueAction.None);
    }

    private void StartItemDelivery(ItemData item)
    {
        itemToDeliver = item;

        SetDialogue(new string[]
        {
            "You've found it. I knew you would.",

            "You have no idea how important this is. Hand it over. We are closer than ever."
        }, DialogueAction.DeliverItem);

        itemToDeliver = item;
    }


    private void StartAfterDelivery()
    {
        string[] lines;
        DialogueAction action = DialogueAction.None;

        switch (QuestManager.Instance.CurrentItemIndex)
        {
            case 1:
                lines = new string[]
                {
                    "Wonderful. The scroll... Do you understand what you've brought me?",
                    "These writings predate most recorded history. Perhaps this sickness is simply something humanity has forgotten.",
                    "One down. Two remain. The gemstone is our next concern."
                };
                break;

            case 2:
                lines = new string[]
                {
                    "Magnificent. The gemstone.",
                    "Do you feel it? Even now, something is stirring. The writings were correct.",
                    "There is something I must prepare before we proceed.",
                    "Take this. Drink it when you are ready. It will prepare your body for what comes next."
                };

                action = DialogueAction.SpawnPotion;
                break;

            default:
                StartEnding();
                return;
        }

        SetDialogue(lines, action);
    }

    private void SpawnPotion()
    {
        QuestManager quest = QuestManager.Instance;

        if (quest == null || quest.PotionSpawned)
            return;

        if (potionPrefab == null || potionSpawnPoint == null)
        {
            Debug.LogError(
                "Potion prefab or spawn point is missing!",
                this
            );
            return;
        }

        Instantiate(
            potionPrefab,
            potionSpawnPoint.position,
            potionSpawnPoint.rotation
        );

        quest.TryMarkPotionSpawned();

        Debug.Log("The Physician spawned the potion.");
    }

    private void StartEnding()
    {
        SetDialogue(new string[]
        {
            "Do you hear that?",

            "That is not the sound of your heart.",

            "It is the sound of something much older.",

            "Humanity has spent centuries praying for salvation. Perhaps it was never meant to receive it.",

            "The cure is ready. Drink it, and the sickness will leave your body. But I fear what its departure may unleash upon the world.",

            "Or destroy it. Deny humanity this salvation, and perhaps spare the world from something far worse.",

            "The choice is yours."
        }, DialogueAction.ChooseEnding);
    }

    public string GetDialogueText()
    {
        if (dialogueLines == null || dialogueLines.Length == 0)
            return "I have nothing more to say.";

        return dialogueLines[dialogueIndex];
    }

    public string GetPrimaryOptionText()
    {
        
        if (dialogueLines == null)
            return "Continue";

        if (dialogueIndex < dialogueLines.Length - 1)
            return "Continue";

        switch (currentAction)
        {
            case DialogueAction.DeliverItem:
                return "Hand it over";

            case DialogueAction.CompleteIntroduction:
                return "Continue";

            case DialogueAction.ChooseEnding:
                return "Take the Cure";

            default:
                return "Understood";
        }
    }

    public bool HasEndingChoices =>
        currentAction == DialogueAction.ChooseEnding &&
        dialogueLines != null &&
        dialogueIndex == dialogueLines.Length - 1;

    public string GetSecondaryOptionText()
    {
        if (HasEndingChoices)
            return "Destroy the Cure";

        return "Goodbye";
    }

    public void SelectPrimaryOption()
    {
        if (dialogueLines == null)
            return;

        // Advance through the current dialogue.
        if (dialogueIndex < dialogueLines.Length - 1)
        {
            dialogueIndex++;
            return;
        }

        // Perform the action after the final dialogue line.
        switch (currentAction)
        {
            case DialogueAction.CompleteIntroduction:
                QuestManager.Instance.CompleteIntroduction();

                StartItemRequest(
                    QuestManager.Instance.CurrentRequiredItem
                );
                break;

            case DialogueAction.DeliverItem:
                DeliverCurrentItem();
                break;

            case DialogueAction.ChooseEnding:
                QuestManager.Instance.TakeCure();
                break;
            
            case DialogueAction.SpawnPotion:
                SpawnPotion();
                currentAction = DialogueAction.None;
                break;
        }
    }

    private void DeliverCurrentItem()
    {
        QuestManager quest = QuestManager.Instance;
        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (quest == null || inventory == null || itemToDeliver == null)
            return;

        if (!quest.CanDeliverItem(itemToDeliver))
            return;

        if (!inventory.RemoveItem(itemToDeliver))
        {
            Debug.LogWarning("Could not remove quest item from inventory!");
            return;
        }

        quest.DeliverItem(itemToDeliver);

        FindFirstObjectByType<InventoryUI>()?.RefreshInventory();

        Debug.Log("Delivered: " + itemToDeliver.itemName);

        if (quest.IsQuestComplete)
        {
            StartEnding();
        }
        else
        {
            StartAfterDelivery();
        }
    }

    public void SelectSecondaryOption()
    {
        if (!HasEndingChoices)
            return;

        QuestManager.Instance.DestroyCure();
    }
}
