
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialogueUI : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;

    [Header("Dialogue Options")]
    [SerializeField] private Button optionButton1;
    [SerializeField] private TMP_Text optionButton1Text;

    [SerializeField] private Button optionButton2;
    [SerializeField] private TMP_Text optionButton2Text;

    private NPCInteraction currentNPC;

    private void Start()
    {
        dialoguePanel.SetActive(false);

        optionButton1.onClick.AddListener(SelectPrimaryOption);
        optionButton2.onClick.AddListener(SelectSecondaryOption);
    }

    public void OpenDialogue(NPCInteraction npc)
    {
        if (npc == null)
            return;

        currentNPC = npc;

        dialoguePanel.SetActive(true);

        npcNameText.text = npc.NPCName;

        currentNPC.BeginDialogue();

        RefreshDialogue();
    }

    public void RefreshDialogue()
    {
        if (currentNPC == null)
            return;

        dialogueText.text = currentNPC.GetDialogueText();

        optionButton1Text.text =
            currentNPC.GetPrimaryOptionText();

        optionButton2Text.text =
            currentNPC.GetSecondaryOptionText();
    }

    private void SelectPrimaryOption()
    {
        if (currentNPC == null)
            return;

        currentNPC.SelectPrimaryOption();

        RefreshDialogue();
    }

    private void SelectSecondaryOption()
    {
        if (currentNPC == null)
            return;

        if (currentNPC.HasEndingChoices)
        {
            currentNPC.SelectSecondaryOption();
        }
        else
        {
            CloseDialogue();
        }
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        currentNPC = null;
    }
}
