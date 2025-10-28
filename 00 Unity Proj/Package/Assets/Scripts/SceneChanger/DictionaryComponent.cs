using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

// This component will serialize dictionaries in the Inspector whilst ensuring that they
// still inherit from MonoBehaviour. The logic is then passed into the attached Scene Changer
// file, whether that be for UI or Collision-based scene changes.

namespace SceneSwitching_cf
{

#if UNITY_EDITOR // Editor namespaces can only be used in the editor.
    using Sirenix.OdinInspector.Editor.Examples;
#endif
    
    public class DictionaryComponent : SerializedMonoBehaviour
    {
        public bool sceneChangeDictionary = true;
        public bool UICanvasDictionary = true;
        public bool PlayerCanvasDictionary = true;

        // Scene Changer Dictionary
        [ShowIfGroup("sceneChangeDictionary")]
        [DetailedInfoBox("How to Use the SceneChanger Dictionary:",
            "This Dictionary Component is split into two parts: key & value. The key box should be filled with " +
            "the GameObject that the player will interact with (collision or UI). The value box should be filled with the " +
            "SceneAsset that you want to switch to. Remember to click Add! These will be passed into the corresponding " +
            "scene changer script that is also attached to the object.")]
        [DictionaryDrawerSettings(KeyLabel = "GameObject", ValueLabel = "SceneAsset")]
        public Dictionary<GameObject, string> SceneChangerDictionary = new Dictionary<GameObject, string>();
        
        // UI Canvas Dictionary
        [ShowIfGroup("UICanvasDictionary")]
        [DetailedInfoBox("How to Use the UI Canvas Dictionary:",
            "This Dictionary Component is split into two parts: key & value. The key box should be filled with " +
            "the Button object that the player will interact with. The value box should be filled with the UI Controller " +
            "of the Canvas that should be displayed (all other canvas objects will be disabled). Remember to click Add! " +
            "These will be passed into the corresponding manager script that is also attached to the object.")]
        [DictionaryDrawerSettings(KeyLabel = "GameObject", ValueLabel = "UIController")]
        public Dictionary<GameObject, UIController> UICanvasControllerDictionary = new Dictionary<GameObject, UIController>();
        
        [ShowIfGroup("PlayerCanvasDictionary")]
        public Dictionary<PlayerInputGameState, UIController> PlayerInputCanvasControllerDictionary = new Dictionary<PlayerInputGameState, UIController>();
    }

}