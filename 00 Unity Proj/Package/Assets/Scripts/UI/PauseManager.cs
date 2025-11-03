using UnityEngine;
using UnityEngine.InputSystem;
using BasicMovement2_cf;

// This class is responsible for Pause Menu management. Popups follow the traditional behavior of
// automatically blocking the input on elements behind it and adding a background texture.
public class PauseManager : MonoBehaviour, PlayerControls.IGameControlsActions
{
    private PlayerControls playerControls; // PlayerControls.cs
    private bool isPaused;
    public GameObject pauseUI;
    
    private void Awake()
    {
        playerControls = new PlayerControls();
    }
    
    private void OnEnable()
    {
        // Enable GameControls
        playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        playerControls.GameControls.Enable();
        
        GameStateManager.gameStateChanged += SetPauseState;
        
        // Initialize the local pause state from the static current state in case
        // this component is enabled after the GameStateManager has already set it.
        // This ensures PauseManager reflects the current state even when it's on a
        // different GameObject.
        SetPauseState(GameStateManager.CurrentGameState);
    }

    private void OnDisable()
    {
        GameStateManager.gameStateChanged -= SetPauseState;
        
        // Disable GameControls
        playerControls.GameControls.Disable();
        playerControls.GameControls.SetCallbacks(null);
    }
    
    private void SetPauseState(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Paused)
        {
            isPaused = true;
            pauseUI.SetActive(true);
        }
        else
        {
            isPaused = false;
            pauseUI.SetActive(false);
        }
    }

    private void Start()
    {
        // Ensure local pause state reflects the current global state (extra safety).
        SetPauseState(GameStateManager.CurrentGameState);
    }
    
    public void OnTogglePause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        
        if (GameStateManager.CurrentGameState == GameStateManager.GameState.Paused)
        {
            GameStateManager.SetGameState(GameStateManager.GameState.Playing);
        }
        else
        {
            GameStateManager.SetGameState(GameStateManager.GameState.Paused);
        }
    }
}