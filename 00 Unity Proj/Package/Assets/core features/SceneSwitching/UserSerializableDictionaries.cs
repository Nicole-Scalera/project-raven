using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
using UnityEditor;
using UnityEngine;

namespace SceneSwitching_cf
{

// Dictionary for linking specfic Scene Objects to specific
// Scene Assets, with respect to scene changes.
    [Serializable]
    public class SceneChangeRoster : SerializableDictionary<GameObject, SceneAsset>
    {
    }

}