using System;
using UnityEngine;
using UnityEngine.UI; // required for Button
using System.Collections.Generic;
using BasicMovement2_cf;
using SceneSwitching_cf;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManagerSaving : MonoBehaviour, PlayerControls.IGameControlsActions
{
    // ===== Variables/Components =====
    //private UIController canvasController; // Specific Canvas controller
    [SerializeField] private UIController defaultCanvasController; // Default Canvas controller
    [SerializeField] private Button[] buttons;
    private GameObject otherObject; // Other object the player interacts with (Button)
    private PlayerInputGameState playerInputGameState; // PlayerInput and GameState tuple
    private GameStateManager.GameState currentState; // Current Game State
    // Player References
    private PlayerControls playerControls; // PlayerControls.cs
    // ================================
    
    // ===== UIController Dictionary =====
    // Create a new instance of a dictionary for a GameObject (button) and a UIController
    private Dictionary<GameObject, UIController> UICanvasControllerDictionary = new();
    private Dictionary<PlayerInputGameState, UIController> PlayerInputCanvasControllerDictionary = new();
    [SerializeField, Required] private DictionaryComponent dictionaryComponent; // Reference to DictionaryComponent.cs
    // ===================================
    
    // ===== Initialization =====
    private void Awake()
    {
        // // Initialize PlayerControls
        // playerControls = new PlayerControls();
        // playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        // playerControls.GameControls.Enable();
        
        // Get the dictionary component from this GameObject or its children
        dictionaryComponent = GetComponent<DictionaryComponent>();
        if (dictionaryComponent == null)
        {
            dictionaryComponent = GetComponentInChildren<DictionaryComponent>();
        }
    }
    
    private void Start()
    {
        GetDictionary(); // Initialize the UIController Dictionary
        CheckForClicks(); // Check for button clicks in the scene
        
        // Set the default canvas on scene start
        _ToggleUICanvas(defaultCanvasController);
    }

    // Get the UI canvas dictionary from DictionaryComponent.cs
    private void GetDictionary()
    {
        if (dictionaryComponent != null)
        {
            // Grab the dictionaries from the object
            UICanvasControllerDictionary = dictionaryComponent.UICanvasControllerDictionary;
            PlayerInputCanvasControllerDictionary = dictionaryComponent.PlayerInputCanvasControllerDictionary;
        }
        else
        {
            Debug.LogWarning("UIManagerSaving > No DictionaryComponent found on this object or its children.");
        }
    }

    private void OnEnable()
    {
        GameStateManager.gameStateChanged += UpdateGameState;
    }
    
    private void OnDisable()
    {
        GameStateManager.gameStateChanged -= UpdateGameState;
    }

    // Update the private reference of the current game state
    private void UpdateGameState(GameStateManager.GameState state)
    {
        currentState = state;
    }
    // ==========================
    
    // =================== UI Input ===================
    // Check for button clicks in the scene
    private void CheckForClicks()
    {
        // Add listeners to each button in the buttons array
        foreach (var button in buttons)
        {
            if (button == null || button.gameObject == null) continue;
            // Add a listener that will pass the button.gameObject to our handler
            button.onClick.AddListener(() => TaskOnClick(button.gameObject));
        }
    }

    // Handle button click events
    private void TaskOnClick(GameObject other)
    {
        Debug.Log("UIManagerSaving > TaskOnClick Triggered by " + other.gameObject.name);
        
        // Set the otherObject to the UI/collided object
        otherObject = other.gameObject;
        
        // Call CheckForKey to see if it is in the dictionary
        CheckForKeyUI(otherObject);
    }
    // ================================================
    
    // =================== UI Input ===================
    // Handle button click events
    public void OnTogglePause(InputAction.CallbackContext context)
    {
        Debug.Log("UIManagerSaving > TaskOnPlayerInput Triggered by " + context.action);
        
        // Get the current game state
        currentState = GameStateManager.CurrentGameState;
        
        // Set the playerInputGameState to the PlayerInput and GameState
        playerInputGameState = new PlayerInputGameState(context.action.actionMap, currentState);
        
        // Call CheckForKeyUI to see if it is in the dictionary
        CheckForKeyPlayerInput(playerInputGameState);
    }
    // ================================================
    
    // ================== Dictionary Logic ==================
    // Call this in TaskOnClick
    private void CheckForKeyUI(GameObject other)
    {
        if (other == null)
        {
            Debug.LogWarning("UIManagerSaving > CheckForKeyUI called with null object");
            return;
        }

        // Try to get the associated UIController from the dictionary safely
        if (UICanvasControllerDictionary != null && UICanvasControllerDictionary.TryGetValue(other, out UIController foundController))
        {
            Debug.Log($"UIManagerSaving > {other.name} Key found in dictionary! Toggling its UIController.");

            // Toggle the associated UI canvas (use the found controller)
            _ToggleUICanvas(foundController);

            // Keep a reference to the last-interacted object and its controller
            otherObject = other;
            return;
        }

        // If we get here, we didn't find a match
        Debug.Log($"UIManagerSaving > {other.name} Key NOT found in dictionary after fallback checks!");
        otherObject = null;
    }
    
    // Call this in TaskOnPlayerInput
    private void CheckForKeyPlayerInput(PlayerInputGameState other)
    {
        if (other.playerInput == null)
        {
            Debug.LogWarning("UIManagerSaving > CheckForKeyPlayerInput called with null PlayerInput");
            return;
        }

        // Try to get the associated UIController from the dictionary safely
        if (PlayerInputCanvasControllerDictionary != null && PlayerInputCanvasControllerDictionary.TryGetValue(other, out UIController foundController))
        {
            Debug.Log($"UIManagerSaving > Key found in dictionary for {other.playerInput} and {other.gameState}! Toggling its UIController.");

            // If the game state is Playing, set to Paused, and vice versa
            GameStateManager.SetGameState(other.gameState == GameStateManager.GameState.Playing ? GameStateManager.GameState.Paused : GameStateManager.GameState.Playing);
            
            // Toggle the associated UI canvas (use the found controller)
            _ToggleUICanvas(foundController);

            // Keep a reference to the last-interacted object and its controller
            playerInputGameState = other;
            return;
        }

        // If we get here, we didn't find a match
        Debug.Log($"UIManagerSaving > Key NOT found in dictionary for {other.playerInput} and {other.gameState}!");
        otherObject = null;
    }
    // ======================================================

    // ========== Toggling Canvases ==========
    // Internal function to toggle the UI canvases: show 'canvas' and hide all others
    void _ToggleUICanvas(UIController canvas)
    {
        if (canvas == null)
        {
            Debug.LogWarning("UIManagerSaving > _ToggleUICanvas called with null canvas. Hiding all dictionary controllers.");

            // If canvas is null, just hide all dictionary controllers
            foreach (var kvp in UICanvasControllerDictionary)
            {
                var c = kvp.Value;
                if (c == null) continue;
                try
                {
                    Debug.Log($"UIManagerSaving > Hiding controller '{c.gameObject?.name ?? c.name}' using index {c.ArraySize()}");
                    c.TogglePanel(c.ArraySize(), false);
                }
                catch (Exception ex) { Debug.LogError($"UIManagerSaving > Error hiding controller '{c.gameObject?.name ?? c.name}': {ex}"); }
            }
            
            return;
        }

        Debug.Log($"UIManagerSaving > Toggling UI Canvas: '{canvas.gameObject?.name ?? canvas.name}' (show) and hiding other dictionary controllers");

        bool shownTargetFromDictionary = false;

        // First, hide all dictionary controllers and mark whether the target was found in the dictionary
        foreach (var kvp in UICanvasControllerDictionary)
        {
            var c = kvp.Value;
            if (c == null) continue;

            if (ReferenceEquals(c, canvas))
            {
                shownTargetFromDictionary = true;
                try
                {
                    Debug.Log($"UIManagerSaving > Showing controller '{c.gameObject?.name ?? c.name}' using index {c.ArraySize()}");
                    c.TogglePanel(c.ArraySize(), true);
                }
                catch (Exception ex) { Debug.LogError($"UIManagerSaving > Error showing controller '{c.gameObject?.name ?? c.name}': {ex}"); }
            }
            else
            {
                try
                {
                    Debug.Log($"UIManagerSaving > Hiding controller '{c.gameObject?.name ?? c.name}' using index {c.ArraySize()}");
                    c.TogglePanel(c.ArraySize(), false);
                }
                catch (Exception ex) { Debug.LogError($"UIManagerSaving > Error hiding controller '{c.gameObject?.name ?? c.name}': {ex}"); }
            }
        }

        // If the requested canvas isn't part of the dictionary, explicitly show it (so defaults work)
        if (!shownTargetFromDictionary)
        {
            try
            {
                Debug.Log($"UIManagerSaving > Requested controller not part of dictionary. Explicitly showing '{canvas.gameObject?.name ?? canvas.name}' using index {canvas.ArraySize()}");
                canvas.TogglePanel(canvas.ArraySize(), true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"UIManagerSaving > Error showing requested controller '{canvas.gameObject?.name ?? canvas.name}': {ex}");
            }
        }
    }
    // =======================================
}