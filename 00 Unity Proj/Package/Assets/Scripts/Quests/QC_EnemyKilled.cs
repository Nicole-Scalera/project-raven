using UnityEngine;

namespace QuestSystem
{
    public class QC_EnemyKilled : QuestComponent
    {
        private int _enemyID;
        private int _killCount;
        private int _killsNeeded;
    
        public QC_EnemyKilled(string name, string description, int enemyID, int killsNeeded) : base(name, description)
        {
            _enemyID = enemyID;
            _killsNeeded = killsNeeded;
            _killCount = 0;
            ComponentType = QuestComponentType.EnemyKilled;
        }
        
        public static QuestComponent CreateFactory(SO_QuestComponent so_questComponent)
        {
            SO_QC_EnemyKilled localQuestComponent = (SO_QC_EnemyKilled)so_questComponent;

            return new QC_EnemyKilled(
                localQuestComponent.componentName,
                localQuestComponent.description,
                localQuestComponent.enemyID,
                localQuestComponent.killsNeeded);
        }

        public override void EnableComponent()
        {
            base.EnableComponent();
            // Subscribe to enemy kill event
        }

        public override void MarkCompleted()
        {
            base.MarkCompleted();
            // Unsubscribe from enemy kill event
        }
    }
}