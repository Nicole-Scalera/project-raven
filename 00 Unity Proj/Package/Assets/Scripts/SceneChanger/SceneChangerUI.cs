using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityCommunity.UnitySingleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// This is the SceneChanger_UI class, which inherits from SceneChanger.cs.
// It is designed to handle scene changes triggered by UI interactions,
// such as button clicks. See SceneChanger_Collision.cs for collision-based
// scene changes.

namespace SceneSwitching_cf {
    public class SceneChangerUI : SceneChanger, ISceneChanger, IClickable
    {
        
        // =================== Event Methods ===================
        // Check for button clicks in the scene
        public override void CheckForClicks()
        {
            // Add listeners to all buttons in the scene
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent<Button>(out Button button) && child.tag == "SceneChangeButton")
                {
                    button.onClick.AddListener(() => TaskOnClick(button.gameObject));
                }
            }
        }

        public override void TaskOnClick(GameObject other)
        {
            Debug.Log("TaskOnClick Triggered by " + other.gameObject.name);
            
            // Set the otherObject to the UI/collided object
            otherObject = other.gameObject;
            
            // Call CheckForKey to see if it is in the dictionary
            CheckForKey(otherObject);
        }
        // =====================================================
    }
}