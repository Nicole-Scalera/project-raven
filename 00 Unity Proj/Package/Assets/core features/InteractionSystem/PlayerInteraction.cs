using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInteraction : MonoBehaviour
{

    public List<GameObject> clues = new List<GameObject>();

    public Transform playerTransform;

    public float distance1;
    public float distance2;

    public void FixedUpdate()
    {
        
        if(clues.Count != 0)
        {

            clues = DistanceSort(clues);

        }

    }

    public void PlayerInteract(InputAction.CallbackContext context)
    {

        Debug.Log("Interacting");

        if (clues.Count > 0)
        {

            if (!clues[0].GetComponent<InteractableClue>().interactedWith)
            {

                Debug.Log(clues[0].GetComponent<InteractableClue>().InteractedWith());

            }
            
        }
    
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.GetComponent<InteractableClue>() != null)
        {

            clues.Add(other.gameObject);

        }

        foreach (var clue in clues)
        {

            Debug.Log(clue);

        }

    }

    public void OnTriggerExit(Collider other)
    {

        Debug.Log(other.gameObject);

        if (other.gameObject.GetComponent<InteractableClue>() != null)
        {

            clues.Remove(other.gameObject);

        }

    }

    private List<GameObject> DistanceSort(List<GameObject> list)
    {

        GameObject closestObject;

        List<GameObject> sortedList = new List<GameObject>();

        float lastDistance  = float.MaxValue;

        for (int i = 0; i < list.Count; i++)
        {

            closestObject = list[i];

            float distance = (playerTransform.position - closestObject.transform.position).sqrMagnitude;

            distance2 = distance;
            distance1 = lastDistance;

            if (distance < lastDistance)
            {

                sortedList.Insert(0,closestObject);
                Debug.Log(sortedList);

            }
            else 
            { 
            
                sortedList.Add(closestObject);
            
            }

                lastDistance = distance;

        }

        return sortedList;

    }

}
