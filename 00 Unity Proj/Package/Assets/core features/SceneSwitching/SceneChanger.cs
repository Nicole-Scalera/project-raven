using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using Sirenix.Utilities;

// This is the SceneChanger class. When called, it will change
// the current scene to a new one of a specific name. Set the
// name in the Inspector, or create a line to manually assign
// it through the code.

namespace SceneSwitching_cf
{
    public abstract class SceneChanger : MonoSingleton<SceneChanger>, ISceneChanger, ICollidable, IClickable
    {

        // ===== Variables/Components =====
        private GameManager gameManager; // Access GameManager.cs
        protected SceneAsset scene;  // The SceneAsset in the Editor
        protected string sceneName; // Name of a scene
        private Rigidbody rb; // Rigidbody (for collisions)
        protected GameObject otherObject; // Other object the player interacts with
        // ================================
        
        // ===== Scene Dictionary =====
        // Create a new instance of a dictionary matching the type from DictionaryComponent.cs
        protected Dictionary<GameObject, SceneAsset> sceneDictionary = new();
        [SerializeField, Required] private DictionaryComponent dictionaryComponent; // Reference to DictionaryComponent.cs
        // ============================

        // ===== Initialization =====
        public virtual void Awake()
        {
            // Assign GameManager.cs
            gameManager = GameManager.Instance;
            dictionaryComponent = GetComponentInChildren<DictionaryComponent>();
        }
        
        public virtual void Start()
        {
            // Initialize the scene changer Dictionary
            GetDictionary();
            CheckForClicks();
        }
        
        // Get the scene dictionary from SceneChangeDictionary.cs
        public virtual void GetDictionary()
        {
            if (dictionaryComponent != null)
            {
                // Grab the dictionary from the object
                sceneDictionary = dictionaryComponent.SceneChangerDictionary;
            }
        }
        // ==========================

        // ================ SceneChanger Methods =================
        // Load a scene by name
        public virtual void LoadScene(string sceneName)
        {
            // Tell the user what scene is being loaded
            Debug.Log($"SceneChanger > Loading scene: {sceneName}...");

            // Load a scene by its name
            SceneManager.LoadScene(sceneName);

            //  Confirm the scene was loaded
            Debug.Log($"SceneChanger > Loaded successfully!");
        }
        
        // Return a string of the current scene
        public string GetCurrentSceneName()
        {
            // Grab the scene by its name
            return SceneManager.GetActiveScene().name;
        }
        
        // Call sceneLoaded when a new scene is loaded
        public virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        // Notify the GameManager of the new "current" scene name
        public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // GameManager always prints whenever a
            // new scene is loaded, included runtime.
            gameManager.UpdateCurrentScene(scene.name);
        }
        // =======================================================
        
        // =================== Event Methods ===================
        // Checking for collisions in a 3D environment
        public virtual void OnCollisionEnter(Collision other)
        {
            // // Set the otherObject to the UI/collided object
            // otherObject = other.gameObject;
            //
            // // Call CheckForKey to see if it is in the dictionary
            // CheckForKey(otherObject);
        }
        
        // Check for button clicks in the scene
        public virtual void CheckForClicks()
        {
            // Handle logic in a derived class
        }

        public virtual void TaskOnClick(GameObject other)
        {
            // Handle logic in a derived class
        }
        // =====================================================
        
        // ================== Dictionary Logic ==================
        // Call this in OnCollisionEnter or CheckForClicks
        public virtual void CheckForKey(GameObject other)
        {
            // Check if the object the Player interacted/collided
            // with is specified in the scene changer dictionary
            if (sceneDictionary.ContainsKey(otherObject))
            {
                Debug.Log($"SceneChanger > {otherObject} Key found in dictionary!");
                
                // Grab the scene associated with the object
                GetDictionaryScene(otherObject);
                
                // Convert the sceneName to a string
                sceneName = scene.name;
                Debug.Log("sceneName: " + sceneName);
                
                // Load the scene by its name
                LoadScene(sceneName);
            }
            else
            {
                // Otherwise, throw out the value
                Debug.Log($"SceneChanger > {otherObject} Key NOT found in dictionary!");
                otherObject = null;
            }
        }
        
        // Get the associated scene from sceneDictionary
        public virtual void GetDictionaryScene(GameObject otherObject)
        {
            Debug.Log($"SceneChanger > Getting scene dictionary: {sceneName}...");
            // Assign the scene from the dictionary
            scene = sceneDictionary[otherObject];
            Debug.Log($"SceneChanger > {sceneName} retrieved.");
        }
        // ======================================================
        
    }
    
    // This interface is used to handle all
    // required SceneChanger methods
    public interface ISceneChanger
    {
        void Awake();
        void Start();
        void GetDictionary();
        void LoadScene(string sceneName);
        string GetCurrentSceneName();
        void OnEnable();
        void OnDisable();
        void OnSceneLoaded(Scene scene, LoadSceneMode mode);
        void CheckForKey(GameObject other);
        void GetDictionaryScene(GameObject otherObject);
    }
    
    // This interface is used to handle collision events
    public interface ICollidable
    {
        void OnCollisionEnter(Collision other);
    }

    // This interface is used to handle button click events
    public interface IClickable
    {
        void CheckForClicks();
        void TaskOnClick(GameObject other);
    }
}