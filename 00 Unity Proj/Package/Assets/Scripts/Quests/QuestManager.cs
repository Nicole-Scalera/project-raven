using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private List<SO_Quest> questsToLoad = new List<SO_Quest>();
        public Dictionary<int, Quest> Quests;

        private void Awake()
        {
            InitializeQuests();
        }

        private void Start()
        {
            // For now, simply start the first quest
            StartQuest(0);
        }

        public bool StartQuest(int id)
        {
            if (Quests == null || !Quests.ContainsKey(id)) return false;
            if (Quests[id].QuestStatus != QuestStatus.Inactive) return false;

            Quests[id].Activate();
            Quests[id].OnQuestCompleted += QuestCompleted;

            Debug.unityLogger.Log($"{Quests[id].QuestName} has been started");
            return true;
        }

        private void QuestCompleted(Quest quest)
        {
            Debug.unityLogger.Log($"{quest.QuestName} has been completed");
            quest.OnQuestCompleted -= QuestCompleted;
            QuestEvents.TriggerQuestCompleted(quest);
        }

        private void InitializeQuests()
        {
            if (questsToLoad.Count <= 0) return;
            
            // Create a dictionary to hold the quests
            Quests = new Dictionary<int, Quest>();
            
            // Loop through each ScriptableObject and create a Quest instance
            foreach (var so in questsToLoad)
            {
                // Load the quest data from the ScriptableObject
                var quest = new Quest(so.questName, so.id, so.components);
                
                // Add the quest to the dictionary
                Quests.Add(quest.QuestID, quest);
                Debug.unityLogger.Log($"{quest.QuestName} has been initialized");
            }
        }
    }
}