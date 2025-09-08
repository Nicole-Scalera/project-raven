// using UnityCommunity.UnitySingleton;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// // This is the SceneChanger class. When called, it will change
// // the current scene to a new one of a specific name. Set the
// // name in the Inspector, or create a line to manually assign
// // it through the code.
//
// namespace SceneSwitching_cf
// {
//     public abstract class SceneChanger2 : MonoSingleton<SceneChanger>, ICollidable, ISceneChanger
//     {
//
//         // ===== Variables/Components =====
//         private GameManager gameManager; // Access GameManager.cs
//         protected Scene scene;  // drag your scene here
//         protected string sceneName; // Name of a scene
//         private Rigidbody rb; // Rigidbody
//         protected string Button; // UI Buttons (assigned in Inspector)
//         protected GameObject otherObject; // Other object player collides with
//         // ================================
//
//         protected virtual void Awake()
//         {
//             gameManager = GameManager.Instance; // Assign GameManager.cs
//         }
//
//         // Load a scene by name (set name in Inspector)
//         public virtual void LoadScene(Scene scene)
//         {
//             // Get the scene asset from the editor and
//             // convert it to a string
//             this.scene = scene;
//             sceneName = scene.name;
//             
//             // Tell the user what scene is being loaded
//             Debug.Log($"SceneChanger > Loading scene: {sceneName}...");
//
//             // Load a scene by its name
//             SceneManager.LoadScene(sceneName);
//
//             //  Confirm the scene was loaded
//             Debug.Log($"SceneChanger > Loaded successfully!");
//         }
//
//         // Get the current scene
//         public virtual string GetCurrentSceneName()
//         {
//             // Grab the scene by its name
//             return SceneManager.GetActiveScene().name;
//         }
//         
//         // In a 3D scene, we have attached this script to the Player.
//         public virtual void OnCollisionEnter(Collision other)
//         {
//             FindCollisionScene();
//             
//             if (other.gameObject.CompareTag(tag))
//             {
//                 otherObject = other.gameObject;
//             }
//             
//             otherObject = other.gameObject;
//             this.scene = scene;
//         }
//         
//         public virtual Scene FindCollisionScene(GameObject otherObject)
//         {
//             
//         }
//         
//         //, GameObject otherObject, TagHandle tag, Scene scene
//         
//         //public virtual void OnCollisionEnter(Collision other)
//         //{
//             // This method is intentionally left blank.
//             // Implement collision handling in derived classes.
//         //}
//         
//
//         // Call sceneLoaded when a new scene is loaded
//         public virtual void OnEnable()
//         {
//             SceneManager.sceneLoaded += OnSceneLoaded;
//         }
//
//         public virtual void OnDisable()
//         {
//             SceneManager.sceneLoaded -= OnSceneLoaded;
//         }
//
//         // Notify the GameManager of the new "current" scene name
//         public virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//         {
//             // GameManager always prints whenever a
//             // new scene is loaded, included runtime.
//             gameManager.UpdateCurrentScene(scene.name);
//         }
//         
//     }
//     
//         // This interface is used to handle UI events for scene changing
//         public interface ISceneChanger
//         {
//             void Awake();
//             void LoadScene(Scene scene);
//             void GetCurrentSceneName();
//             void OnEnable();
//             void OnDisable();
//             void OnSceneLoaded(Scene scene, LoadSceneMode mode);
//     
//         }
//     
//     // This interface is used to handle collision events
//     public interface ICollidable
//     {
//         void OnCollisionEnter(Collision other);
//     }
//
// }