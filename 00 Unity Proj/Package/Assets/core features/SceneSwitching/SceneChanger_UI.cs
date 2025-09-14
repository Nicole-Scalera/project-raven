using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

// This is the SceneChanger_UI class, which inherits from SceneChanger2.cs.
// It is designed to handle scene changes triggered by UI interactions,
// such as button clicks. See SceneChanger_Collision.cs for collision-based
// scene changes.

namespace SceneSwitching_cf {
    public class SceneChanger_UI : SceneChanger2, ISceneChanger, IClickable
    {
        // ===== Variables/Components =====
        protected SC_UIRoster sceneDictionary; // UI dictionary
        // ================================
        
        // Get the scene dictionary from SceneChangeDictionary.cs
        public virtual void GetDictionary()
        {
            sceneDictionary = GetComponent<SC_UIRoster>();
        }
        
        // =================== Event Methods ===================
        // Check for button clicks in the scene
        public virtual void CheckForClicks()
        {
            // Handle logic in a derived class
        }

        public virtual void TaskOnClick(string buttonName)
        {
            // Handle logic in a derived class
        }
        // =====================================================
    }
}