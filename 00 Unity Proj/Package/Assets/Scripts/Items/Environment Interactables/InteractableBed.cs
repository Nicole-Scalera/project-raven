using UnityEngine;

public class InteractableBed : MonoBehaviour, IInteractable
{

    public GameObject frontDoor;
    public GameObject global;

    private void Start()
    {

        frontDoor = GameObject.FindGameObjectWithTag("Door");
        global = GameObject.FindGameObjectWithTag("Global");

    }

    public void Interaction()
    {
        if (global.GetComponent<InterSceneData>().factoryCompleted)
        {
            frontDoor.GetComponent<InteractableApartmentDoor>().unlocked = true;
            global.GetComponent<InterSceneData>().factoryCompleted = false;

            //TODO FADE OUT AND IN


        }
        Debug.Log("Go to Sleep");

    }
}
