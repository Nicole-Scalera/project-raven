using UnityEngine;

// Objective: collect a specific item a certain number of times.

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "QC_ItemCollected", menuName = "QuestSystem/Components/QC_ItemCollected", order = 3)]
    public class SO_QC_ItemCollected : SO_QuestComponent
    {
        [Header("Item Settings")]
        public int itemID;
        public int itemAmountNeeded;
    }
}
