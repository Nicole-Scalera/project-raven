using System;
using UnityEngine;

public class InteractableClue : MonoBehaviour, IInteractable
{
    public string clueName;
    public bool interactedWith = false;

    public string Interaction()
    {

        interactedWith = true;
        return clueName;

    }

}
