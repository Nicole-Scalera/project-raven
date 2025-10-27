using System;
using Sirenix.OdinInspector;
using UnityEngine;

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
    }
    
    private void OnEnable()
    {
        gameStateChanged += OnGameStateChanged;
    }
    
    private void OnDisable()
    {
        gameStateChanged -= OnGameStateChanged;
    }
    
    // Static method to set the game state and notify subscribers
    public static void SetGameState(GameState newState)
    {
        // Notify all subscribers (including component instances)
        gameStateChanged?.Invoke(newState);
    }
    
    // When the state has been changed, update the local variable
    // and print out a debug message
    private void OnGameStateChanged(GameState newState)
    {
        gameState = newState;
        Debug.Log("GameStateManager > Game State changed to: " + newState.ToString());
    }
    
    
}