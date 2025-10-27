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
        // Get the dictionary component from this GameObject or its children
        dictionaryComponent = GetComponent<DictionaryComponent>();
        if (dictionaryComponent == null)
        {
            dictionaryComponent = GetComponentInChildren<DictionaryComponent>();
        }

        // Ensure the dictionaries are populated early (input can fire as soon as Awake)
        GetDictionary();
        
        // Initialize PlayerControls
        playerControls = new PlayerControls();
        playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        playerControls.GameControls.Enable();
        
        currentState = GameStateManager.CurrentGameState;
    }
    
    private void Start()
    {
        CheckForClicks(); // Check for button clicks in the scene
        
        // Set the default canvas on scene start
        _ToggleCanvasUI(defaultCanvasController);
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
        if (!context.performed)
            return;
        
        // Safely read action and action map names from the context to avoid NullReferenceException
        var action = context.action;
        var actionName = action?.name;
        var actionMapName = context.action?.actionMap?.name;

        Debug.Log("UIManagerSaving > TaskOnPlayerInput Triggered by action: " + actionName + " on map: " + actionMapName);

        if (string.IsNullOrEmpty(actionMapName))
        {
            Debug.LogWarning("UIManagerSaving > OnTogglePause: actionMap name is null or empty. Aborting.");
            return;
        }

        // Compute the current game state we're in
        var current = GameStateManager.CurrentGameState;

        // Create the key using the action map and the current game state
        playerInputGameState = new PlayerInputGameState(actionMapName, current);

        // Call CheckForKey to see if it is in the dictionary (using the target state)
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
            _ToggleCanvasUI(foundController);

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
        // Null check
        if (other.actionMap == null)
        {
            Debug.LogWarning("UIManagerSaving > CheckForKeyPlayerInput called with null actionMap name");
            return;
        }

        // First try the fast dictionary lookup
        if (PlayerInputCanvasControllerDictionary != null && PlayerInputCanvasControllerDictionary.TryGetValue(other, out UIController foundController))
        {
            Debug.Log($"UIManagerSaving > Key found in dictionary for {other.actionMap} and {other.gameState}! Toggling its UIController.");

            // Toggle the associated UI canvas (use the found controller) and pass actionMap + the (now current) game state
            _ToggleCanvasPlayerInput(foundController, other.actionMap, other.gameState);
            
            // If the game state is Playing, set to Paused, and vice versa
            GameStateManager.SetGameState(other.gameState == GameStateManager.GameState.Playing ? GameStateManager.GameState.Paused : GameStateManager.GameState.Playing);

            // Keep a reference to the last-interacted object and its controller
            playerInputGameState = other;
            return;
        }

        // If TryGetValue failed, attempt a safe manual lookup (handles possible hash/serialization mismatches)
        if (PlayerInputCanvasControllerDictionary != null)
        {
            foreach (var kvp in PlayerInputCanvasControllerDictionary)
            {
                var key = kvp.Key;
                if (key == null) continue;

                bool mapMatches = string.Equals(key.actionMap, other.actionMap, StringComparison.Ordinal);
                bool stateMatches = key.gameState == other.gameState;

                Debug.Log($"UIManagerSaving > Manual check: key.actionMap='{key.actionMap}', key.gameState='{key.gameState}', mapMatches={mapMatches}, stateMatches={stateMatches}");

                if (mapMatches && stateMatches)
                {
                    var controller = kvp.Value;
                    Debug.Log($"UIManagerSaving > Manual lookup matched key for {other.actionMap} and {other.gameState}. Using controller '{controller?.gameObject?.name ?? controller?.name}'");
                    
                    _ToggleCanvasPlayerInput(controller, key.actionMap, key.gameState);
                    
                    // If the game state is Playing, set to Paused, and vice versa
                    GameStateManager.SetGameState(other.gameState == GameStateManager.GameState.Playing ? GameStateManager.GameState.Paused : GameStateManager.GameState.Playing);

                    playerInputGameState = other;
                    return;
                }
            }
        }

        // If we get here, we didn't find a match
        Debug.Log($"UIManagerSaving > Key NOT found in dictionary for {other.actionMap} and {other.gameState}!");
        otherObject = null;
    }
    // ======================================================

    // ========== Toggling Canvases ==========
    // Internal function to toggle the UI canvases: show 'canvas' and hide all others
    void _ToggleCanvasUI(UIController canvas)
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
                Debug.Log($"You requested {canvas} controller.");
                Debug.Log($"UIManagerSaving > Requested controller not part of dictionary. Explicitly showing '{canvas.gameObject?.name ?? canvas.name}' using index {canvas.ArraySize()}");
                canvas.TogglePanel(canvas.ArraySize(), true);
            }
            catch (Exception ex)
            {
                Debug.LogError($"UIManagerSaving > Error showing requested controller '{canvas.gameObject?.name ?? canvas.name}': {ex}");
            }
        }
    }
    
    
    // Internal function to toggle UI canvas with PlayerInputGameState
    void _ToggleCanvasPlayerInput(UIController canvas, string actionMap, GameStateManager.GameState gameState)
    {
        if (canvas == null)
        {
            Debug.LogWarning("UIManagerSaving > _ToggleUICanvas called with null canvas. Hiding all player-input dictionary controllers.");

            // If canvas is null, just hide all dictionary controllers (player input dictionary)
            foreach (var kvp in PlayerInputCanvasControllerDictionary)
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

        Debug.Log($"UIManagerSaving > Toggling UI Canvas (PlayerInput): '{canvas.gameObject?.name ?? canvas.name}' (show) and hiding other player-input dictionary controllers");
        
        bool shownTargetFromDictionary = false;

        // Iterate the PlayerInput dictionary and compare the actionMap string + gameState to decide show/hide
        foreach (var kvp in PlayerInputCanvasControllerDictionary)
        {
            var key = kvp.Key;
            var c = kvp.Value;
            if (c == null || key == null) continue;

            bool keyMatches = string.Equals(key.actionMap, actionMap, StringComparison.Ordinal);
            bool stateMatches = key.gameState == gameState;

            if (keyMatches && stateMatches && ReferenceEquals(c, canvas))
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
                Debug.Log($"UIManagerSaving > Requested controller not part of player-input dictionary. Explicitly showing '{canvas.gameObject?.name ?? canvas.name}' using index {canvas.ArraySize()}");
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