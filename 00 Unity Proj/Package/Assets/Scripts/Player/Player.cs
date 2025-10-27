using System;
using Unity.VisualScripting;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.InputSystem;

// This is the Player class that will allow us to get specific data about the Player.

public class Player : MonoSingleton<Player>
{
    [SerializeField] private Rigidbody rb; // Rigidbody
    [SerializeField] private PlayerInput _playerInput;
    public static event Action inputMapSwitched;

    void Awake()
    {
        if (_playerInput == null)
        {
            _playerInput = GetComponent<PlayerInput>();
        }
    }
    
    // Returns the PlayerInput component attached to the player
    public PlayerInput GetPlayerInput()
    {
        return _playerInput;
    }

    // Subscribe to events
    private void OnEnable()
    {
        if (_playerInput != null)
        {
            inputMapSwitched += CurrentActionMap;
            GameStateManager.gameStateChanged += SwitchActionMap;
            GameStateManager.gameStateChanged += SwitchCursorFunctionality;
        }
    }

    // Unsubscribe from events
    private void OnDisable()
    {
        if (_playerInput != null)
        {
            inputMapSwitched += CurrentActionMap;
            GameStateManager.gameStateChanged -= SwitchActionMap;
            GameStateManager.gameStateChanged -= SwitchCursorFunctionality;
        }
    }
    
    // Switches the current action map to the specified action map name
    private void SwitchActionMap(GameStateManager.GameState newState)
    {
        if (_playerInput != null)
        {
            string actionMapName = newState == GameStateManager.GameState.Playing ? "PlayerMove" : "UI";
            _playerInput.SwitchCurrentActionMap(actionMapName);
            inputMapSwitched?.Invoke();
        }
    }
    
    // Switches cursor functionality based on game state
    private void SwitchCursorFunctionality(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Playing)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (newState == GameStateManager.GameState.Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    // Prints out the name of the current action map
    private void CurrentActionMap()
    {
        Debug.Log("Player > CurrentActionMap > Current Action Map: " + _playerInput.currentActionMap.name);
    }
    
}