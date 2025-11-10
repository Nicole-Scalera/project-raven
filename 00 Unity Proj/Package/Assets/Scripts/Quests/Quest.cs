using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace QuestSystem
{
    public enum QuestStatus { Inactive, Active, Completed };

    public class Quest
    {
        public event Action<Quest> OnQuestCompleted;

        public readonly string QuestName;
        public int QuestID;
        public QuestStatus QuestStatus;
        public List<QuestComponent> QuestComponents = new List<QuestComponent>();

        private int _componentsCompleted;

        private readonly Dictionary<QuestComponent.QuestComponentType, Func<SO_QuestComponent, QuestComponent>>
            _componentFactory = new Dictionary<QuestComponent.QuestComponentType, Func<SO_QuestComponent, QuestComponent>>
            {
                { QuestComponent.QuestComponentType.EnemyKilled, QC_EnemyKilled.CreateFactory },
                { QuestComponent.QuestComponentType.ItemCollected, QC_ItemCollected.CreateFactory }
            };

        public Quest(string name, int id, List<SO_QuestComponent> questComponents)
        {
            QuestName = name;
            QuestID = id;
            QuestStatus = QuestStatus.Inactive;

            if (questComponents == null || questComponents.Count <= 0)
                return;

            foreach (SO_QuestComponent questComponent in questComponents)
            {
                QuestComponent qcTemp = null;

                if (_componentFactory.ContainsKey(questComponent.questType))
                    qcTemp = _componentFactory[questComponent.questType](questComponent);

                if (qcTemp == null)
                    return;

                QuestComponents.Add(qcTemp);

                // Subscribe to the component so that the Quest knows
                // when the component has been completed.
                qcTemp.OnComponentCompleted += ComponentCompleted;
            }
        }

        ~Quest()
        {
            // Unsubscribe to avoid leaks
            for (int i = QuestComponents.Count - 1; i >= 0; i--)
            {
                if (QuestComponents[i] != null)
                    QuestComponents[i].OnComponentCompleted -= ComponentCompleted;
                QuestComponents[i] = null;
            }
        }

        private void ComponentCompleted(QuestComponent questComponent)
        {
            _componentsCompleted++;

            Debug.unityLogger.Log($"{QuestName}: Component '{questComponent.ComponentName}' was completed");

            if (_componentsCompleted == QuestComponents.Count)
            {
                QuestStatus = QuestStatus.Completed;
                OnQuestCompleted?.Invoke(this);
            }
            else
            {
                // If there are more components left,
                // activate the next component
                if (QuestComponents.Count > _componentsCompleted)
                {
                    QuestComponents[_componentsCompleted].EnableComponent();
                }
            }
        }

        public void Activate()
        {
            if (QuestComponents.Count < 1) return;

            QuestComponents[0].EnableComponent();
            QuestStatus = QuestStatus.Active;
        }
    }
}
