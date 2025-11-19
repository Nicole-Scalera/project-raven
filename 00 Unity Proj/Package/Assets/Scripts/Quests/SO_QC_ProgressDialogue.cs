using UnityEngine;

// Objective: Progress through dialogue.

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "QC_ProgressDialogue", menuName = "QuestSystem/Components/QC_ProgressDialogue", order = 4)]
    public class SO_QC_ProgressDialogue : SO_QuestComponent
    {
        [Header("Item Settings")]
        public int dialogueID;
    }
}
