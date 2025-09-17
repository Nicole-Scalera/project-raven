using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{

    public List<GameObject> interactables = new List<GameObject>();

    public Collider interactionZone;

    public void PlayerInteract(InputAction.CallbackContext context)
    {

        Debug.Log("Interacting");

        if (interactables.Count > 0)
        {

            Debug.Log(interactables[0].GetComponent<Interactable>().InteractedWith());

        }
    
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<Interactable>() != null)
        {

            interactables.Add(other.gameObject);

        }

        foreach (var interactable in interactables)
        {

            Debug.Log(interactable);

        }

    }

    public void OnTriggerExit(Collider other)
    {

        Debug.Log(other.gameObject);

        interactables.Remove(other.gameObject);

    }

}
