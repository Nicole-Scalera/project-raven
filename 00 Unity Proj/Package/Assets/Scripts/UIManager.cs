using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BasicMovement2_cf;

public class UIManager : MonoBehaviour, PlayerControls.IGameControlsActions
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
    private Player player; // Player instance
    private PlayerControls playerControls; // PlayerControls.cs
    private PlayerInput playerInput; // PlayerInput component
    
    void Awake()
    {
        // Initialize PlayerControls
        playerControls = new PlayerControls();
        playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        playerControls.GameControls.Enable();
        
        player = Player.Instance; // Get the Player instance
        playerInput = player.GetPlayerInput(); // Get the PlayerInput component from the Player instance
        
        showMenuCanvas = false; // Hide UI elements
        showGameplayCanvas = true; // Show gameplay UI
        
        // Default InputActionMap should be PlayerMove
        Debug.Log(playerInput.currentActionMap);
    
    }

    void Start()
    {
        // Debug Cursor Info
        Debug.Log("Cursor Visibility: " + Cursor.visible);
        Debug.Log("Cursor Lock State: " + Cursor.lockState);
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
            Debug.Log("PlayerInput currently set to " + playerInput.currentActionMap);
            pauseMenu.TogglePanel(pauseMenu.ArraySize(), true);
            gameplayUI.TogglePanel(gameplayUI.ArraySize(), false);
            
            // Make the cursor available for UI interaction
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            
            // Switch to the UI Action Map
            playerInput.SwitchCurrentActionMap("UI");
            Debug.Log("PlayerInput now set to " + playerInput.currentActionMap);
        }
        else
        {
            Debug.Log("PlayerInput currently set to " + playerInput.currentActionMap);
            pauseMenu.TogglePanel(pauseMenu.ArraySize(), false);
            gameplayUI.TogglePanel(gameplayUI.ArraySize(), true);
            
            // Hide & lock the cursor for gameplay
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            
            // Switch to the PlayerMove Action Map
            playerInput.SwitchCurrentActionMap("PlayerMove");
            Debug.Log("PlayerInput now set to " + playerInput.currentActionMap);
        }

        this.showMenuCanvas = show;
    }
    // ===================================

    void OnDestroy()
    {
        playerControls.GameControls.Disable();
    }
}