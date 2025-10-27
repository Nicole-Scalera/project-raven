using System.Collections.Generic;
using System;

// I created this instead of using a Tuple<> to pass in more than one variable as a key
// in a SerializedDictionary search, specifically in the DictionaryComponent.cs script.

[System.Serializable]
public class PlayerInputGameState : IEquatable<PlayerInputGameState>
{
    public string actionMap; // Player Input reference
    public GameStateManager.GameState gameState; // Is the game Paused/Playing?

    public PlayerInputGameState(string map, GameStateManager.GameState state)
    {
        actionMap = map;
        gameState = state;
    }

    // Value equality so this object can be used as a Dictionary key reliably
    public bool Equals(PlayerInputGameState other)
    {
        if (ReferenceEquals(this, other)) return true;
        if (other is null) return false;
        return string.Equals(actionMap, other.actionMap, StringComparison.Ordinal) && gameState == other.gameState;
    }

    public override bool Equals(object obj) => Equals(obj as PlayerInputGameState);

    public override int GetHashCode()
    {
        unchecked
        {
            int hash = 17;
            hash = hash * 23 + (actionMap != null ? actionMap.GetHashCode() : 0);
            hash = hash * 23 + gameState.GetHashCode();
            return hash;
        }
    }

    public static bool operator ==(PlayerInputGameState left, PlayerInputGameState right) => EqualityComparer<PlayerInputGameState>.Default.Equals(left, right);
    public static bool operator !=(PlayerInputGameState left, PlayerInputGameState right) => !(left == right);
}