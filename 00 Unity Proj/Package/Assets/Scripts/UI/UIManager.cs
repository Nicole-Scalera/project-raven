using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BasicMovement2_cf;

// This is the UIManager script that currently manages the Pause Menu and Gameplay UI.
// I plan on making it more dynamic in the future to encompass the switching of other
// UI canvases, but right now it focuses on just pausing/playing.

public class UIManager : MonoBehaviour//, PlayerControls.IGameControlsActions
{
    // UI Controllers for individual UI canvases in the scene.
    // Panels under the canvas object are assigned in the Inspector
    // and are toggled on/off as needed in UIController.cs.
    public UIController pauseMenu; // Pause Menu UI Controller
    public UIController gameplayUI; // Gameplay UI Controller
    
    // Booleans
    private bool isPaused = false; // Is the game paused?
    private bool showMenuCanvas; // Toggles the pause menu
    private bool showGameplayCanvas; // Toggles the gameplay UI
    
    // Player References
    private PlayerControls playerControls; // PlayerControls.cs
    
    void Awake()
    {
        // Initialize PlayerControls
        //playerControls = new PlayerControls();
        //playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        //playerControls.GameControls.Enable();
        
        showMenuCanvas = false; // Hide UI elements
        showGameplayCanvas = true; // Show gameplay UI
    }
    
    // ========== Check for Triggering Menu ==========
    public void OnTogglePause(InputAction.CallbackContext context)
    {
        // If NOT paused and the Escape is triggered
        if (context.performed && !isPaused)
        {
            Debug.Log("Game is paused.");
            isPaused = true;
            Time.timeScale = 0;
            TogglePauseMenu(showMenuCanvas = true); // Enable the pause menu
        }

        // If paused and the Escape is triggered
        else if (context.performed && isPaused)
        {
            Debug.Log("Game is not paused.");
            isPaused = false;
            Time.timeScale = 1;
            TogglePauseMenu(showMenuCanvas = false); // Disable the pause menu
        }
    }
    // ===============================================
    
    // ========== Toggling Menu ==========
    void OnEnable()
    {
        _TogglePauseMenu(showMenuCanvas);
    }
    
    // Turn the pause menu on/off
    public void TogglePauseMenu(bool show)
    {
        _TogglePauseMenu(show);
    }

    // Internal function to toggle the pause menu
    void _TogglePauseMenu(bool show)
    {
        Debug.Log("Toggling pause menu: " + show);
        
        if (show == true)
        {
            // Update the game state to Paused
            GameStateManager.SetGameState(GameStateManager.GameState.Paused);
            // Note that this method from GameStateManager will broadcast a message to any
            // subscribers so that they can change their components and behaviors accordingly.
            
            // Update the UI panels
            pauseMenu.TogglePanel(pauseMenu.ArraySize(), true);
            gameplayUI.TogglePanel(gameplayUI.ArraySize(), false);
        }
        else
        {
            // Update the game state to Playing
            GameStateManager.SetGameState(GameStateManager.GameState.Playing);
            
            // Update the UI panels
            pauseMenu.TogglePanel(pauseMenu.ArraySize(), false);
            gameplayUI.TogglePanel(gameplayUI.ArraySize(), true);
        }

        this.showMenuCanvas = show;
    }
    // ===================================

    void OnDestroy()
    {
        //playerControls.GameControls.Disable();
    }
}