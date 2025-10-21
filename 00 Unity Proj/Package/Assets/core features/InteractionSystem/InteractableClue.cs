using System;
using UnityEngine;

public class InteractableClue : MonoBehaviour
{
    public string clueName;
    public bool interactedWith = false;
    

    // private void Start()
    // {
    //     // string actionMapName = newState == GameStateManager.GameState.Playing ? "PlayerMove" : "UI";
    //     // bool wasCollected = interactedWith ? true : false;
    //     // interactedWith = wasCollected;
    // }

    private void OnEnable()
    {
        PlayerInteraction.clueCollected += Interaction;
        // PlayerInteraction.clueCollected += OnClueCollected;
    }
    
    private void OnDisable()
    {
        PlayerInteraction.clueCollected -= Interaction;
        // PlayerInteraction.clueCollected -= OnClueCollected;
    }

    // Call this when a clue is interacted with
    private void Interaction(InteractableClue interactableClue)
    {
        // Set its interactedWith property to true
        Debug.Log("InteractableClue > Interaction() executed with this clue: " + clueName);
        interactedWith = true;
    }
    
    // When the state has been changed, update the local variable
    // and print out a debug message
    // private void OnClueCollected(InteractableClue clue)
    // {
    //     interactedWith = true;
    //     // gameState = newState;
    //     // Debug.Log("GameStateManager > Game State changed to: " + newState.ToString());
    // }
}
