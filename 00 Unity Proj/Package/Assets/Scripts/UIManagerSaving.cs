using System;
using UnityEngine;
using UnityEngine.UI; // required for Button
using System.Collections.Generic;
using SceneSwitching_cf;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;

public class UIManagerSaving : MonoBehaviour
{
    // ===== Variables/Components =====
    //private UIController canvasController; // Specific Canvas controller
    [SerializeField] private UIController defaultCanvasController; // Default Canvas controller
    [SerializeField] private Button[] buttons;
    private GameObject otherObject; // Other object the player interacts with (Button)
    // ================================
    
    // ===== UIController Dictionary =====
    // Create a new instance of a dictionary for a GameObject (button) and a UIController
    private Dictionary<GameObject, UIController> canvasControllerDictionary = new();
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
            // Grab the dictionary from the object
            canvasControllerDictionary = dictionaryComponent.UICanvasControllerDictionary;
        }
        else
        {
            Debug.LogWarning("UIManagerSaving > No DictionaryComponent found on this object or its children.");
        }
    }
    // ==========================
    
    // =================== Event Methods ===================
    // Check for button clicks in the scene
    private void CheckForClicks()
    {
        // Add listeners to all Button components under this transform (safer than relying on tags)
        // var buttons = GetComponentsInChildren<Button>(true);

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
        
        // Debug.Log("TaskOnClick Triggered by " + other.gameObject.name);
        //     
        // // Set the otherObject to the UI/collided object
        // otherObject = other.gameObject;
        
        // Call CheckForKey to see if it is in the dictionary
        CheckForKey(otherObject);
    }
    // =====================================================
    
    // ================== Dictionary Logic ==================
    // Call this in CheckForClicks
    private void CheckForKey(GameObject other)
    {
        if (other == null)
        {
            Debug.LogWarning("UIManagerSaving > CheckForKey called with null object");
            return;
        }

        // Try to get the associated UIController from the dictionary safely
        if (canvasControllerDictionary != null && canvasControllerDictionary.TryGetValue(other, out UIController foundController))
        {
            Debug.Log($"UIManagerSaving > {other.name} Key found in dictionary! Toggling its UIController.");

            // Toggle the associated UI canvas (use the found controller)
            _ToggleUICanvas(foundController);

            // Keep a reference to the last-interacted object and its controller
            otherObject = other;
            // canvasController = foundController;
            return;
        }

        // Fallback matching: maybe the passed GameObject is a child (or parent) of the key used in the dictionary,
        // or the names match. Try to find a reasonable candidate.
        if (canvasControllerDictionary != null)
        {
            foreach (var kvp in canvasControllerDictionary)
            {
                var key = kvp.Key;
                var controller = kvp.Value;
                if (key == null) continue;

                // 1) exact reference match (already tried, but keep for clarity)
                if (ReferenceEquals(key, other))
                {
                    Debug.Log($"UIManagerSaving > Found dictionary key by reference for {other.name}.");
                    _ToggleUICanvas(controller);
                    otherObject = key;
                    //canvasController = controller;
                    return;
                }

                // 2) name match
                if (string.Equals(key.name, other.name, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Log($"UIManagerSaving > Found dictionary key by name match: '{key.name}' for '{other.name}'.");
                    _ToggleUICanvas(controller);
                    otherObject = key;
                    //canvasController = controller;
                    return;
                }

                // 3) hierarchy match: other is child of key, or key is child of other
                if (other.transform.IsChildOf(key.transform) || key.transform.IsChildOf(other.transform))
                {
                    Debug.Log($"UIManagerSaving > Found dictionary key by hierarchy match: '{key.name}' for '{other.name}'.");
                    _ToggleUICanvas(controller);
                    otherObject = key;
                    //canvasController = controller;
                    return;
                }
            }
        }

        // If we get here, we didn't find a match
        Debug.Log($"UIManagerSaving > {other.name} Key NOT found in dictionary after fallback checks!");
        otherObject = null;
    }
    
    // Get the associated UIController from canvasDictionary
    private void GetDictionaryCanvasController(GameObject keyObject)
    {
        if (keyObject == null)
        {
            Debug.LogWarning("UIManagerSaving > GetDictionaryCanvasController called with null object");
            //canvasController = null;
            return;
        }

        if (canvasControllerDictionary != null && canvasControllerDictionary.TryGetValue(keyObject, out UIController found))
        {
            Debug.Log($"UIManagerSaving > UIController '{found.gameObject?.name ?? found.name}' retrieved from dictionary for '{keyObject.name}'.");
            //canvasController = found;
        }
        else
        {
            Debug.LogWarning($"UIManagerSaving > No UIController found in dictionary for {keyObject.name}.");
            //canvasController = null;
        }
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
            foreach (var kvp in canvasControllerDictionary)
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

            //canvasController = null;
            return;
        }

        Debug.Log($"UIManagerSaving > Toggling UI Canvas: '{canvas.gameObject?.name ?? canvas.name}' (show) and hiding other dictionary controllers");

        bool shownTargetFromDictionary = false;

        // First, hide all dictionary controllers and mark whether the target was found in the dictionary
        foreach (var kvp in canvasControllerDictionary)
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

        // Store the currently shown canvas reference
        //canvasController = canvas;
    }
    // =======================================
}