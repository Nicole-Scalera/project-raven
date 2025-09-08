using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using UnityEngine;

namespace SceneSwitching_cf
{

// Default "string" : "string" dictionary
    [Serializable]
    public class StringStringDictionary : SerializableDictionary<string, string>
    {
    }

// Dictionary for linking specfic UI elements with specific
// Scene Assets, with respect to button-based scene changes.
    [Serializable]
    public class SC_UIRoster : SerializableDictionary<GameObject, SceneAsset>
    {
    }

// Dictionary for linking specific Game Objects with specific
// Scene Assets, with respect to collision-based scene changes. 
    [Serializable]
    public class SC_CollisionRoster : SerializableDictionary<GameObject, SceneAsset>
    {
    }

}