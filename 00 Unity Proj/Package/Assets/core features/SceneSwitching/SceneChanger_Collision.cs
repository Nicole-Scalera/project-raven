using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is the SceneChanger_Collision class, which inherits from
// SceneChanger2.cs. It is designed to handle scene changes triggered
// by collision-based interactions. See SceneChanger_UI.cs for
// UI-based scene changes.

namespace SceneSwitching_cf {
    public class SceneChanger_Collision : SceneChanger2, ISceneChanger, ICollidable
    {
        // ===== Variables/Components =====
        protected SC_CollisionRoster sceneDictionary; // Collision dictionary
        // ================================
        
        // Get the scene dictionary from SceneChangeDictionary.cs
        public virtual void GetDictionary()
        {
            sceneDictionary = GetComponent<SC_CollisionRoster>();
        }
        
        // =================== Event Methods ===================
        // Checking for collisions in a 3D environment
        public virtual void OnCollisionEnter(Collision other)
        {
            // Set the otherObject to the UI/collided object
            otherObject = other.gameObject;
            
            // Call CheckForKey to see if it is in the dictionary
            CheckForKey(otherObject);
        }
        // =====================================================
    }
}