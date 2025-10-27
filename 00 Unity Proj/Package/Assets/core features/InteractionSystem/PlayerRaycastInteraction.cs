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

    //     ==== Active Interactable Item ====

    public GameObject activeInteractable;

    //========================================================

    //     ==== Player Raycast ====

    public Vector3 mousePos;
    public Ray interactionRay;
    public RaycastHit raycastHit;
    public bool isHitting;
    public float rayLength = 5f;

    //     ==== Interaction States ====

    public bool hasBat = false;
    public GameObject activeBat = null;

    //========================================================

    /*
     * 
     * - Grabs player mouse position then creates a ray starting at the current camera location
     * - Emits the ray using Physics.Raycast and uses isHitting to track collision
     * - If the ray is hitting the ray is emitted green and checks if the colliding
     *   object is interactable and if so assigns it to the activeInteractable variable
     *      - If its a clue an added check is performed to ensure the clue hasn't already been interactedWith
     * - If it isnt hitting the ray is emitted red and assigns null to activeInteractable
     * 
     */
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

            if (raycastHit.collider.GetComponent<InteractableBox>() != null)
            {

                activeInteractable = raycastHit.collider.gameObject;

            }
            else if (raycastHit.collider.GetComponent<InteractableClue>() != null && !raycastHit.collider.GetComponent<InteractableClue>().interactedWith)
            {

                activeInteractable = raycastHit.collider.gameObject;

            }
            else if (raycastHit.collider.GetComponent<InteractableBat>() != null)
            {

                activeInteractable = raycastHit.collider.gameObject;

            }

        }
        else
        {

            Debug.DrawRay(origin, direction * rayLength, Color.red);

            if (activeInteractable != null && activeInteractable.GetComponent<InteractableBox>() == null)
            {

                activeInteractable = null;

            }

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
        if(activeInteractable != null)
        {

            if (activeInteractable.GetComponent<InteractableBox>() != null)
            {

                activeInteractable.GetComponent<InteractableBox>().Interaction();

            }
            else if (activeInteractable.GetComponent<InteractableClue>() != null && !raycastHit.collider.GetComponent<InteractableClue>().interactedWith)
            {

                activeInteractable.GetComponent<InteractableClue>().Interaction();

            }
            else if (activeInteractable.GetComponent<InteractableBat>() != null)
            {

                activeInteractable.GetComponent<InteractableBat>().Interaction();
                hasBat = activeInteractable.GetComponent<InteractableBat>().interactedWith;
                activeBat = activeInteractable;

            }

        }
        else if (activeBat != null)
        {

            activeBat.GetComponent<InteractableBat>().Interaction();
            hasBat = activeBat.GetComponent<InteractableBat>().interactedWith;
            activeBat = null;

        }
    }

}
