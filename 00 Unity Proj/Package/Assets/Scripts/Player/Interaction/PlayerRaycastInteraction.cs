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
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PlayerRaycastInteraction : MonoBehaviour
{
    
    //     ===== Clues List ======

    public List<GameObject> clues = new List<GameObject>();

    //========================================================

    //     ==== Active Interactable Item ====

    public GameObject activeInteractable;

    //========================================================

    //     ==== Player Raycast ====

    public Vector3 halfDimensions = new(1f,1f,1f);
    public Vector3 mousePos;
    public Ray interactionRay;
    public RaycastHit boxcastHit;
    public bool isHitting;
    public float rayLength = 5f;

    //     ==== Interaction States ====

    public bool hasBat = false;
    public bool hasTape = false;
    public GameObject activeBat = null;
    public GameObject activeTape = null;

    //========================================================

    //      ==== Game UI ====

    public GameObject uiDot;
    public Sprite interactableSprite;
    public Sprite defaultDotSprite;

    /*
     *
     * - Grabs player mouse position then creates a ray starting at the current camera location to the mouse position
     * - Emits the box using Physics.Boxcast using the ray as a template and uses isHitting to track collision
     * - If the box is hitting the ray is emitted green and checks if the colliding
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

        isHitting = Physics.BoxCast(origin,halfDimensions,direction, out boxcastHit,transform.rotation,rayLength);


        if (isHitting)
        {

            Debug.DrawRay(origin, direction * rayLength, Color.green);

            Debug.Log(boxcastHit.collider.gameObject.name);

            if (boxcastHit.collider.TryGetComponent<IInteractable>(out _))
            {
                uiDot.GetComponent<UnityEngine.UI.Image>().color = Color.yellow;
                activeInteractable = boxcastHit.collider.gameObject;
            }

        }
        else
        {

            Debug.DrawRay(origin, direction * rayLength, Color.red);
            uiDot.GetComponent<UnityEngine.UI.Image>().color = Color.white;

            
            if (activeBat != null)
            {
                
                if (activeInteractable.GetComponent<BeatenBox>() == null)
                {

                    activeInteractable = null;

                }

            }
            else
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
            else if(activeInteractable.GetComponent<BeatenBox>() != null)
            {

                activeInteractable.GetComponent <BeatenBox>().Interaction();

            }
            else if(activeInteractable.GetComponent<TapedBox>() != null)
            {

                activeInteractable.GetComponent<TapedBox>().Interaction();

            }
            else if (activeInteractable.GetComponent<InteractableClue>() != null && !boxcastHit.collider.GetComponent<InteractableClue>().interactedWith)
            {

                activeInteractable.GetComponent<InteractableClue>().Interaction();

            }
            else if (activeInteractable.GetComponent<InteractableBat>() != null)
            {

                activeInteractable.GetComponent<InteractableBat>().Interaction();
                hasBat = activeInteractable.GetComponent<InteractableBat>().interactedWith;
                activeBat = activeInteractable;

            }
            else if (activeInteractable.GetComponent<InteractableTape>() != null)
            {

                activeInteractable.GetComponent<InteractableTape>().Interaction();
                hasTape = activeInteractable.GetComponent<InteractableTape>().interactedWith;
                activeTape = activeInteractable;

            }

        }
        else if (activeBat != null)
        {

            activeBat.GetComponent<InteractableBat>().Interaction();
            hasBat = activeBat.GetComponent<InteractableBat>().interactedWith;
            activeBat = null;

        }
        else if (activeTape != null)
        {

            activeTape.GetComponent<InteractableTape>().Interaction();
            hasTape = activeTape.GetComponent<InteractableTape>().interactedWith;
            activeTape = null;

        }
    }

}
