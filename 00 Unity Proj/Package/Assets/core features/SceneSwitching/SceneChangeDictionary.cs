using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneSwitching_cf
{
	public class SceneChangeDictionary : MonoBehaviour
	{

		[SerializeField] SceneChangeRoster m_sceneChangeRoster = null;

		// This will create a dictionary in the Unity Editor that contains
		// a key and a value for each collidable object. For instance, if
		// you collide with Door1, you should switch to Scene2. Rather
		// than hardcoding it in the scripts, we can set them in the
		// editor by dragging and dropping the Game Objects and the Scene
		// Assets into the specific Dictionary entry.
		public IDictionary<GameObject, SceneAsset> SceneChangeRoster
		{
			get { return m_sceneChangeRoster; }
			set { m_sceneChangeRoster.CopyFrom(value); }
		}

		void Reset()
		{
			// // access by property
			// StringStringDictionary = new Dictionary<string, string>()
			// 	{ { "first key", "value A" }, { "second key", "value B" }, { "third key", "value C" } };
		}

		private void Start()
		{
			
			foreach (var definition in SceneChangeRoster)
			{
				Debug.Log(definition.Key + " : " + definition.Value);
			}

		}

	}
}