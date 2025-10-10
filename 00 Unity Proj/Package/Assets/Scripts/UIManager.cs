using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using BasicMovement2_cf;

public class UIManager : MonoBehaviour, PlayerControls.IGameControlsActions
{
    
    /// <summary>
    /// The main UI object which used for the menu.
    /// </summary>
    public UIController pauseMenu;
    public UIController gameplayUI;
    
    // /// <summary>
    // /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
    // /// </summary>
    // public Canvas[] gameplayUI;
    
    // Booleans
    private bool isPaused = false; // Boolean to check if game is paused
    
    private bool showMenuCanvas;
    private bool showGameplayCanvas;

    private PlayerControls playerControls; // PlayerControls.cs
    
    private InputAction m_MenuAction;
    
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
            ToggleMainMenu(showMenuCanvas = true); // Enable the pause menu
        }

        // If paused and the Escape is triggered
        else if (context.performed && isPaused)
        {
            Debug.Log("Game is not paused.");
            isPaused = false;
            Time.timeScale = 1;
            ToggleMainMenu(showMenuCanvas = false); // Disable the pause menu
        }
    }
    // ===============================================
    

    // ========== Pause & Resume ==========
    // ====================================
    
    // ========== Toggling Menu ==========
    void OnEnable()
    {
        _ToggleMainMenu(showMenuCanvas);
        Debug.Log("showMenuCanvas: " + showMenuCanvas);
        Debug.Log("isPaused: " + isPaused);
        playerControls.Enable();
    }
    
    /// <summary>
    /// Turn the main menu on or off.
    /// </summary>
    /// <param name="show"></param>
    public void ToggleMainMenu(bool show)
    {
        _ToggleMainMenu(show);
    }

    // Internal function to toggle the main menu
    void _ToggleMainMenu(bool show)
    {
        Debug.Log("Toggling main menu: " + show);
        
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