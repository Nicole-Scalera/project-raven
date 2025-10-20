using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerRaycastInteraction : MonoBehaviour
{
    
    //     ===== Clues List ======

    public List<GameObject> clues = new List<GameObject>();

    //========================================================

    //     ==== Active Box ====

    public GameObject activeBox;

    //========================================================

    //     ==== Interactable List ====

    public List<GameObject> interactableObjects = new List<GameObject>();

    //========================================================

    //     ==== Player Reference Transform ====

    public Transform playerTransform;

    //========================================================

    //     ==== Player Raycast ====

    public Vector3 mousePos;
    public Ray interactionRay;
    public RaycastHit raycastHit;
    public bool isHitting;
    public float rayLength = 5f;

    // If there are objects in the clues list, the list constantly
    // sorts the GameObjects in the list by distance using DistanceSort()
    public void FixedUpdate()
    {
        
        mousePos = Input.mousePosition;

        interactionRay = GetComponentInChildren<Camera>().ScreenPointToRay(mousePos);

        Vector3 origin = new (interactionRay.origin.x, interactionRay.origin.y - 0.25f, interactionRay.origin.z);
        Vector3 direction = interactionRay.direction;

        isHitting = Physics.Raycast(interactionRay, out raycastHit);

        if (isHitting)
        {

            Debug.DrawRay(origin, direction * rayLength, Color.green);

            Debug.Log(raycastHit.collider.GetComponent<InteractableBox>() != null);

            if (raycastHit.collider.GetComponent<InteractableBox>() != null)
            {

                raycastHit.collider.GetComponent<InteractableBox>().Interaction();
                activeBox.GetComponent<Rigidbody>().useGravity = !activeBox.GetComponent<InteractableBox>().interactedWith;

            }

        }
        else
        {

            Debug.DrawRay(origin, direction * rayLength, Color.red);
            interactableObjects.Clear();
            //Debug.Log("Cleared");

        }

    }

    /* 
     * When the player presses the interact button, it checks if there is
     * an object to interact with and if it has already been interacted with
     * then if it exists and not interacted with it runs the InteractedWith()
     * function of the object    
     */

    public void PlayerInteract(InputAction.CallbackContext context)
    {

        //Debug.Log("Interacting");

        Debug.Log(!interactableObjects.IsNullOrEmpty());

        if (activeBox != null)
        {

            Debug.Log("In Objects");

            //DistanceSort(interactableObjects); // Call to find the nearest object

            // Checks for an InteractableClue
            if (interactableObjects[0].GetComponent<InteractableClue>() != null)
            {  
                // Run Interaction() from InteractableClue.cs
                interactableObjects[0].GetComponent<InteractableClue>().Interaction();

            }
            // Checks for an InteractableBox
            else if (interactableObjects[0].GetComponent<InteractableBox>() != null)
            {
                // Set the activeBox to the nearest box
                activeBox = interactableObjects[0];

                // Run Interaction() from InteractableBox.cs
                activeBox.GetComponent<InteractableBox>().Interaction();

                // Disable gravity on that box if it's picked up
                activeBox.GetComponent<Rigidbody>().useGravity = !activeBox.GetComponent<InteractableBox>().interactedWith;

            }
        }
    }

}
