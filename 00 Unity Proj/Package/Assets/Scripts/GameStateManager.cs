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
    
    // Static event to notify subscribers of game state changes
    public static event Action<GameState> gameStateChanged;
    
    // Available game states
    public enum GameState { Playing, Paused }
    
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