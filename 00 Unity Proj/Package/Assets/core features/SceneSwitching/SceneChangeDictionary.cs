using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SceneSwitching_cf
{
	public class SceneChangeDictionary : MonoBehaviour
	{
		// The dictionaries can be accessed throught a property
		[SerializeField] StringStringDictionary m_stringStringDictionary = null;

		[SerializeField] SC_UIRoster m_sceneUIRoster = null;

		[SerializeField] SC_CollisionRoster m_sceneCollisionRoster = null;

		public IDictionary<string, string> StringStringDictionary
		{
			get { return m_stringStringDictionary; }
			set { m_stringStringDictionary.CopyFrom(value); }
		}

		// This will create a dictionary in the Unity Editor that contains
		// a key and a value for each collidable object. For instance, if
		// you collide with Door1, you should switch to Scene2. Rather
		// than hardcoding it in the scripts, we can set them in the
		// editor by dragging and dropping the Game Objects and the Scene
		// Assets into the specific Dictionary entry.
		public IDictionary<GameObject, SceneAsset> SceneCollisionRoster
		{
			get { return m_sceneCollisionRoster; }
			set { m_sceneCollisionRoster.CopyFrom(value); }
		}

		void Reset()
		{
			// access by property
			StringStringDictionary = new Dictionary<string, string>()
				{ { "first key", "value A" }, { "second key", "value B" }, { "third key", "value C" } };
		}

		private void Start()
		{
			foreach (var definition in StringStringDictionary)
			{
				Debug.Log(definition.Key + " : " + definition.Value);
			}

			foreach (var definition in SceneCollisionRoster)
			{
				Debug.Log(definition.Key + " : " + definition.Value);
			}

		}

	}
}