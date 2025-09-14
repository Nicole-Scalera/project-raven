using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace SceneSwitching_cf
{

#if UNITY_EDITOR // Editor namespaces can only be used in the editor.
    using Sirenix.OdinInspector.Editor.Examples;
#endif

    [InfoBox(
        "This component will serialize dictionaries in the Inspector whilst ensuring that they still inherit from MonoBehaviour.")]
    public class DictionaryComponent : SerializedMonoBehaviour
    {
        [InfoBox(
            "In order to serialize dictionaries, all we need to do is to inherit our class from SerializedMonoBehaviour.")]
        [DictionaryDrawerSettings(KeyLabel = "Custom Key Name", ValueLabel = "Custom Value Label")]
        public Dictionary<GameObject, SceneAsset> SceneChangerDictionary = new Dictionary<GameObject, SceneAsset>();

        // private static readonly GameObject[] _gameObjects = new GameObject[3]
        // {
        //     new GameObject("GameObject"),
        //     new GameObject("AnotherGameObject"),
        //     new GameObject("ThirdGameObject")
        // };

#if UNITY_EDITOR // Editor-related code must be excluded from builds
        [OnInspectorInit]
        private void CreateData()
        {
            // SceneChangerDictionary = new Dictionary<GameObject, SceneAsset>()
            // {
            //     { new GameObject("GameObject"), new SceneAsset("Scene") }
            // };

        }
#endif

    }
}