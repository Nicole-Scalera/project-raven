using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <remarks>
/// Attach this script to UI Canvas objects to control the toggling of UI elements in the scene. Note that
/// general UI logic is handled in UIManager.cs, but methods are called from here to toggle the actual
/// visibility state of specific panels that are unique to each UI Canvas object.
/// </remarks>
/// </summary>

public class UIController : MonoBehaviour
{
    // Stores a series of UI objects
    [SerializeField] private GameObject[] panels;

    // Toggles the active/inactive state of UI objects
    public void TogglePanel(int index, bool active)
    {
        for (var i = 0; i < panels.Length; i++)
        {
            var g = panels[i];

            // If the current state of the object is not
            // the same as the active parameter, update it
            if (g.activeSelf != active)
            {
                g.SetActive(active);
            }
        }
    }
    
    // Returns how many UI objects are in the array
    public int ArraySize()
    {
        return panels.Length;
    }

    void OnEnable()
    {
        TogglePanel(0, false);
    }
}