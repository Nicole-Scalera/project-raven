using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

// I created this instead of using a Tuple<> to pass in more than one variable as a key
// in a SerializedDictionary search, specifically in the DictionaryComponent.cs script.

[System.Serializable]
public struct PlayerInputGameState
{
    public PlayerInput playerInput; // Player Input reference
    public GameStateManager.GameState gameState; // Is the game Paused/Playing?

    public PlayerInputGameState(PlayerInput input, GameStateManager.GameState state)
    {
        playerInput = input;
        gameState = state;
    }
}