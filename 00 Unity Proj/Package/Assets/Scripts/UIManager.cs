using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BasicMovement2_cf;

public class UIManager : MonoBehaviour, PlayerControls.IGameControlsActions
{
    public UIController pauseMenu; // Pause Menu UI Controller
    public UIController gameplayUI; // Gameplay UI Controller
    
    // Booleans
    private bool isPaused = false; // Is the game paused?
    private bool showMenuCanvas; // Toggles the pause menu
    private bool showGameplayCanvas; // Toggles the gameplay UI

    private PlayerControls playerControls; // PlayerControls.cs
    
    void Awake()
    {
        // Initialize PlayerControls
        playerControls = new PlayerControls();
        playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        playerControls.GameControls.Enable();
        
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
        Debug.Log("showMenuCanvas: " + showMenuCanvas);
        Debug.Log("isPaused: " + isPaused);
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
            pauseMenu.TogglePanel(pauseMenu.ArraySize(), true);
            gameplayUI.TogglePanel(gameplayUI.ArraySize(), false);
        }
        else
        {
            pauseMenu.TogglePanel(pauseMenu.ArraySize(), false);
            gameplayUI.TogglePanel(gameplayUI.ArraySize(), true);
        }

        this.showMenuCanvas = show;
    }
    // ===================================

    void OnDestroy()
    {
        playerControls.GameControls.Disable();
    }
}