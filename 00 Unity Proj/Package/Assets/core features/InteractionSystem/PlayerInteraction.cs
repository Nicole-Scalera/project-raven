using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using Sirenix.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInteraction : MonoBehaviour
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


    // if there are objects in the clues list the list constantly
    // sorts the GameObjects in the list by distance using DistanceSort()
    public void FixedUpdate()
    {
        
        if(clues.Count != 0)
        {

            clues = DistanceSort(clues);

        }

    }

    /* 
     * When the player presses the interact button, it checks if there is
     * an object to interact with and if it has already been interacted with
     * then if it it exists and not interacted with it runs the InteractedWith()
     * function of the object    
     */

    public void PlayerInteract(InputAction.CallbackContext context)
    {

        Debug.Log("Interacting");

        if (!interactableObjects.IsNullOrEmpty())
        {
            DistanceSort(interactableObjects);

            if (interactableObjects[0].GetComponent<InteractableClue>() != null)
            {

                interactableObjects[0].GetComponent<InteractableClue>().Interaction();

            }
            else if (interactableObjects[0].GetComponent<InteractableBox>() != null)
            {

                activeBox = interactableObjects[0];

                activeBox.GetComponent<InteractableBox>().Interaction();

                activeBox.GetComponent<Rigidbody>().useGravity = !activeBox.GetComponent<InteractableBox>().interactedWith;

            }
        }

    }

    /*
     * 
     * When a collider enteres the collider marked as a trigger
     * it checks if the collider's GameObject has the script
     * "InteractableClue" and if so adds the GameObject to the
     * clues list
     * 
     */

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<InteractableClue>() != null)
        {

            clues.Add(other.gameObject);

            interactableObjects.Add(other.gameObject);

        }
        else if (other.gameObject.GetComponent<InteractableBox>() != null)
        {

            activeBox = other.gameObject;

            interactableObjects.Add(other.gameObject);

        }

    }

    /*
     * 
     * When a collider exits the collider marked as a trigger
     * it checks if the collider's GameObject has the script
     * "InteractableClue" and if so removes the GameObject to the
     * clues list
     * 
     */

    public void OnTriggerExit(Collider other)
    {

        if (other.gameObject.GetComponent<InteractableClue>() != null)
        {

            clues.Remove(other.gameObject);
            interactableObjects.Remove(other.gameObject);

        }
        else if (other.gameObject.GetComponent<InteractableBox>() != null)
        {

            activeBox = null;
            interactableObjects.Remove(other.gameObject);
        }

    }

    /*
     * 
     * This is a custom sorting function that takes in a list of
     * GameObjects. It has variables for the sorted list and the
     * previous distance from the player. It then loops through
     * the list and checks the distance between the player and
     * current GameObject. It then checks if it has already been 
     * interacted with if so it adds it to the back of the list
     * and then it checks if it is closer the previous distance, 
     * and if so it inserts it at the front of the list. If not 
     * it adds it to the back of the list. The function then
     * returns the sorted list
     * 
     */

    private List<GameObject> DistanceSort(List<GameObject> list)
    {

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

        return sortedList;

    }

}
