using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is the SceneChanger_Collision class, which inherits from
// SceneChanger.cs. It is designed to handle scene changes triggered
// by collision-based interactions. See SceneChanger_UI.cs for
// UI-based scene changes.

namespace SceneSwitching_cf {
    public class SceneChangerCollision : SceneChanger, ISceneChanger, ICollidable
    {
        // =================== Event Methods ===================
        // Checking for collisions in a 3D environment
        public override void OnCollisionEnter(Collision other)
        {
            Debug.Log("OnCollisionEnter Triggered by " + other.gameObject.name);
            
            // Set the otherObject to the UI/collided object
            otherObject = other.gameObject;
            
            // Call CheckForKey to see if it is in the dictionary
            CheckForKey(otherObject);
        }
        // =====================================================
    }
}