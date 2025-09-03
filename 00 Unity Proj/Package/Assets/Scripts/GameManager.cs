using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is the GameManager class. It is used for various general purposes in
// the game, such as keeping track of the current scene name. It is also a
// helpful point of reference for other scripts to access commonly used
// variables. It will grow and expand over time.

public class GameManager : PersistentMonoSingleton<GameManager>
{
    private SceneChanger sceneChanger; // SceneChanger.cs
    private string currentScene; // Current Scene Name
                // ^^^ The currentScene variable is called anytime
                // a scene is loaded (including runtime).
    
    private void Awake()
    {
        // ===== References =====
        sceneChanger = SceneChanger.Instance; // Access SceneChanger.cs
                    // ^^^ Notice how SceneChanger is also using the
                    // UnitySingleton package, just like GameManager.
        
        // Get the name of the current scene
        currentScene = sceneChanger.GetCurrentSceneName();
    }

    // Update the current scene name everytime a new scene is loaded
    public void UpdateCurrentScene(string sceneName)
    {
        currentScene = sceneName;
        Debug.Log($"GameManager > Current scene is: " + currentScene);
    }

}
