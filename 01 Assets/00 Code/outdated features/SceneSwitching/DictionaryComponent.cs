using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using Sirenix.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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
        [DetailedInfoBox("How to Use?",
        "This Dictionary Component is split into two parts: key & value. The key box should be filled with" +
        "the GameObject that the player will interact with (collision or UI). The value box should be filled with the" +
        "SceneAsset that you want to switch to. Remember to click Add! These will be passed into the corresponding" +
        "scene changer script that is also attached to the object.")]
        [DictionaryDrawerSettings(KeyLabel = "GameObject", ValueLabel = "SceneAsset")]
        public Dictionary<GameObject, string> SceneChangerDictionary = new Dictionary<GameObject, string>();
    }
    


}