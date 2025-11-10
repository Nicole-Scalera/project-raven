using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = "Quest", menuName = "QuestSystem/Quest", order = 0)]
    public class SO_Quest : ScriptableObject
    {
        public string questName;
        public int id;
        public List<SO_QuestComponent> components;
    }
}