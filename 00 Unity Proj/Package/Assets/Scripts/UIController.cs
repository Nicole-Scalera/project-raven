using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    // Stores a series of UI objects
    public GameObject[] panels;

    // Toggles the visibility of the UI objects
    public void SetActivePanel(int index)
    {
        for (var i = 0; i < panels.Length; i++)
        {
            var active = i == index;
            var g = panels[i];
            if (g.activeSelf != active) g.SetActive(active);
        }
    }

    void OnEnable()
    {
        SetActivePanel(0);
    }
}