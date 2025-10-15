using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is the GameManager class. It is used for various general purposes in
// the game, such as keeping track of the current scene name. It is also a
// helpful point of reference for other scripts to access commonly used
// variables. It will grow and expand over time.

namespace SceneSwitching_cf
{
    public class GameManager : PersistentMonoSingleton<GameManager>
    {
        private string currentScene; // Current Scene Name
        // ^^^ The currentScene variable is called anytime
        // a scene is loaded (including runtime).
        
        private void OnEnable()
        {
            // Subscribe GameManager methods to SceneManager events
            SceneManager.sceneLoaded += GetCurrentScene;
            SceneManager.sceneLoaded += VerifyTime;
            SceneManager.activeSceneChanged += ChangedActiveScene;
        }
        
        private void OnDisable()
        {
            // Unsubscribe methods from SceneManager events
            SceneManager.sceneLoaded -= GetCurrentScene;
            SceneManager.sceneLoaded -= VerifyTime;
            SceneManager.activeSceneChanged -= ChangedActiveScene;
        }

        // Update the current scene name everytime a new scene is loaded
        public void GetCurrentScene(Scene scene, LoadSceneMode mode)
        {
            // Update and debug the current scene name
            currentScene = SceneManager.GetActiveScene().name;
            Debug.Log($"GameManager > GetCurrentScene > Current scene is: " + currentScene);
        }
        
        // The scene has been changed from one to another
        private void ChangedActiveScene(Scene current, Scene next)
        {
            string currentName = current.name; // Name of the current scene

            if (currentName == null)
            {
                currentName = "Replaced";
            }

            // Debug the scene change
            Debug.Log("GameManager > ChangedActiveScene > Scenes: " + currentName + ", " + next.name);
        }

        // Ensure that the game is running at a normal speed and is
        // not paused whenever a new scene is loaded
        public void VerifyTime(Scene scene, LoadSceneMode mode)
        {
            float timeScale = Time.timeScale;
            
            // Ensure the game is running at normal speed
            if (timeScale != 1)
            {
                Time.timeScale = 1;
            }
            
            // Debug the timescale
            Debug.Log("GameManager > VerifyTime > Time is set to: " + Time.timeScale);
            
        }

    }
}