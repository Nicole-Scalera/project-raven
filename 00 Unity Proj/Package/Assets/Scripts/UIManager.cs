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
    
    /// <summary>
    /// A list of canvas objects which are used during gameplay (when the main ui is turned off)
    /// </summary>
    public Canvas[] gamePlayCanvasii;
    
    // Booleans
    private bool canPause = true; // Boolean to check if game can be paused
    private bool isPaused = false; // Boolean to check if game is paused
    
    bool showMenuCanvas = false;

    private PlayerControls playerControls; // PlayerControls.cs
    
    private InputAction m_MenuAction;
    
    void Awake()
    {
        // Initialize PlayerControls
        playerControls = new PlayerControls();
        playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        playerControls.GameControls.Enable();
        
        // Hide UI elements
        showMenuCanvas = false;
    }
    
    // ========== Check for Triggering Menu ==========
    public void OnTogglePause(InputAction.CallbackContext context)
    {
        // If NOT paused and the Escape is triggered
        if (context.started && canPause)
        {
            PauseGame(); // Pause the game
        }

        // If paused and the Escape is triggered
        else if (context.started && isPaused)
        {
            ResumeGame(); // Resume the game
        }
    }
    // ===============================================
    

    // ========== Pause & Resume ==========
    void PauseGame()
    {
        Debug.Log("Game is paused");
        isPaused = true;
        Time.timeScale = 0;
        canPause = false;
        ToggleMainMenu(showMenuCanvas = true); // Enable the pause menu
    }

    // Call to Resume the game
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        canPause = true;
        ToggleMainMenu(showMenuCanvas = false); // Disable the pause menu
    }
    // ====================================
    
    // ========== Toggling Menu ==========
    void OnEnable()
    {
        _ToggleMainMenu(showMenuCanvas);
        playerControls.Enable();
    }
    
    /// <summary>
    /// Turn the main menu on or off.
    /// </summary>
    /// <param name="show"></param>
    public void ToggleMainMenu(bool show)
    {
        if (this.showMenuCanvas != show)
        {
            _ToggleMainMenu(show);
        }
    }

    // Internal function to toggle the main menu
    void _ToggleMainMenu(bool show)
    {
        if (show)
        {
            Time.timeScale = 0;
            pauseMenu.gameObject.SetActive(true);
            foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            pauseMenu.gameObject.SetActive(false);
            foreach (var i in gamePlayCanvasii) i.gameObject.SetActive(true);
        }
        this.showMenuCanvas = show;
    }
    // ===================================

    void OnDestroy()
    {
        playerControls.GameControls.Disable();
    }
}