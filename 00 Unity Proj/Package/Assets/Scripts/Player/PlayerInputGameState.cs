using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

// I created this instead of using a Tuple<> to pass in more than one variable as a key
// in a SerializedDictionary search, specifically in the DictionaryComponent.cs script.

[System.Serializable]
public class PlayerInputGameState
{
    public InputActionAsset playerInput; // Player Input reference
    public InputActionMap inputMap;
    public GameStateManager.GameState gameState; // Is the game Paused/Playing?
    
    [ValueDropdown("TreeViewOfInts", ExpandAllMenuItems = true)]
    public List<InputActionMap> ActionMaps = new List<InputActionMap>();
    
    // playerInput.actionMaps.Select(m => m.name).ToList();

    public PlayerInputGameState(InputActionMap map, GameStateManager.GameState state)
    {
        inputMap = map;
        gameState = state;
        
        // List<string> MapNames;
        //
        // ActionMaps = playerInput.actionMaps.Select(m => m.name).ToString();
        //
        // inputMap = map;
        //
        //
        //   
        // // playerInput = playerInput.actionMaps.Select(map => map.name).ToList();
        //
        // //playerInput = map;
        // gameState = state;
    }

}