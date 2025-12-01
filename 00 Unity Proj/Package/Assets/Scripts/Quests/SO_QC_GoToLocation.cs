using UnityEngine;

// Objective: go to a specific location.

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "QC_GoToLocation", menuName = "QuestSystem/Components/QC_GoToLocation", order = 2)]
    public class SO_QC_GoToLocation : SO_QuestComponent
    {
        [Header("Item Settings")]
        public int itemID;
        public int itemAmountNeeded;
    }
}
