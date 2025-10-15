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

        }
        else
        {

            Debug.DrawRay(origin, direction * rayLength, Color.red);

        }

        if(clues.Count != 0)
        {
            // Call to sort the clues list by distance
            clues = DistanceSort(clues);

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

        Debug.Log("Interacting");

        if (!interactableObjects.IsNullOrEmpty())
        {
            DistanceSort(interactableObjects); // Call to find the nearest object

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

    /*
     * When a collider enters the collider marked as a trigger,
     * it checks if the collider's GameObject has the script
     * "InteractableClue," and if so, it adds the GameObject to
     * the clues list defined earlier.
     */

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<InteractableClue>() != null)
        {
            clues.Add(other.gameObject); // Add to clues list
            interactableObjects.Add(other.gameObject); // Add to interactable list
        }
        else if (other.gameObject.GetComponent<InteractableBox>() != null)
        {
            activeBox = other.gameObject; // Set the activeBox to the box entered
            interactableObjects.Add(other.gameObject); // Add to interactable list
        }

    }

    /*
     * When a collider exits the collider marked as a trigger
     * it checks if the collider's GameObject has the script
     * "InteractableClue" and if so removes the GameObject to the
     * clues list
     */

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<InteractableClue>() != null)
        {
            clues.Remove(other.gameObject); // Remove from clues list
            interactableObjects.Remove(other.gameObject); // Remove from interactable list
        }
        else if (other.gameObject.GetComponent<InteractableBox>() != null)
        {
            activeBox = null; // Clear the activeBox
            interactableObjects.Remove(other.gameObject); // Remove from interactable list
        }
    }

    /*
     * This is a custom sorting function that takes in a list of
     * GameObjects. It has variables for the sorted list and the
     * previous distance from the player. It then loops through
     * the list and checks the distance between the player and
     * current GameObject. It then checks if it has already been 
     * interacted with if so it adds it to the back of the list
     * and then it checks if it is closer the previous distance, 
     * and if so it inserts it at the front of the list. If not 
     * it adds it to the back of the list. The function then
     * returns the sorted list.
     */

    private List<GameObject> DistanceSort(List<GameObject> list)
    {

        // Initializes a new list to store the sorted objects
        List<GameObject> sortedList = new List<GameObject>();

        float lastDistance  = float.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {

            float distance = (playerTransform.position - list[i].transform.position).sqrMagnitude;

            if (list[i].GetComponent<InteractableClue>() != null)
            {
                if (list[i].GetComponent<InteractableClue>().interactedWith)
                {

                    sortedList.Add(list[i]);

                }

            }
            else if (list[i].GetComponent<InteractableBox>() != null)
            {

                if (list[i].GetComponent<InteractableBox>().interactedWith)
                {

                    sortedList.Insert(0, list[i]);

                }

            }
            else if (distance < lastDistance)
            {

                sortedList.Insert(0, list[i]);
                Debug.Log(sortedList);
                lastDistance = distance;

            }
            else
            {

                sortedList.Add(list[i]);

            }

        }

        return sortedList; // Spits out the sorted list

    }

}
