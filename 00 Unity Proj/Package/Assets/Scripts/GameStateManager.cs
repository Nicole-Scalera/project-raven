using System;
using Sirenix.OdinInspector;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.SceneManagement;

// This is the GameStateManager class, which manages the game's state (e.g., Playing,
// Paused) and notifies other components of state changes. It uses an enum to define
// the possible game states and provides a static event to broadcast state changes.

public class GameStateManager : MonoBehaviour
{
    [Title("Game State")]
    [EnumToggleButtons, HideLabel]
    [InfoBox("Choose the default game state that this scene will load in.")]
    public GameState gameState;
    // Note: This variable is not static because each instance of the component
    // needs to maintain its own copy of the current game state for reference. Also,
    // the Enum will clearly change when the static event is invoked, so watch out
    // for the Inspector.
    
    // Available game states
    public enum GameState { Playing, Paused }

    // Static property to get the current game state
    public static GameState CurrentGameState { get; set; }
    
    // Static event to notify subscribers of game state changes
    public static event Action<GameState> gameStateChanged;

    private void Awake()
    {
        // Get the value of the enum from the editor
        Debug.Log("GameStateManager > Selected Game State: " + gameState.ToString());

        // Ensure the static CurrentGameState reflects the inspector selection as early as possible.
        // This helps other components (that may be on different GameObjects) read the correct
        // starting state in their OnEnable/Start.
        SetGameState(gameState);
    }
    
    private void OnEnable()
    {
        gameStateChanged += OnGameStateChanged;
        SceneManager.sceneLoaded += SceneDefaults;
    }
    
    private void OnDisable()
    {
        gameStateChanged -= OnGameStateChanged;
        SceneManager.sceneLoaded -= SceneDefaults;
    }
    
    // By default the scene will load in the selected game state
    public void SceneDefaults(Scene scene, LoadSceneMode mode)
    {
        // Initialize the static CurrentGameState from the inspector value
        SetGameState(gameState);
    }
    
    // Static method to set the game state and notify subscribers
    public static void SetGameState(GameState newState)
    {
        Debug.unityLogger.Log("GameStateManager > SetGameState " + newState.ToString());
        
        // Update the static current state
        CurrentGameState = newState;

        // Notify all subscribers (including component instances)
        gameStateChanged?.Invoke(newState);
    }
    
    // When the state has been changed, update the local variable
    // and print out a debug message
    private void OnGameStateChanged(GameState newState)
    {
        gameState = newState;
        
        // Set time scale based on game state
        if (newState == GameState.Paused)
        {
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
        }
    }
}