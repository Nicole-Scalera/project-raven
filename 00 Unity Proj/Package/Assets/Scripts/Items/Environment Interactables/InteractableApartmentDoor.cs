using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableApartmentDoor : MonoBehaviour, IInteractable
{

    public bool unlocked = false;
    public GameObject global;

    private void Start()
    {
        global = GameObject.FindGameObjectWithTag("Global");

        if (global.GetComponent<InterSceneData>().factoryDay == 0)
        {
            unlocked = true;
        }
        else
        {
            unlocked = false;
        }

    }

    public void Interaction()
    {
        if (unlocked)
        {

            global.GetComponent<InterSceneData>().factoryDay += 1;
            unlocked = false;
            SceneManager.LoadScene("Test_Factory");

        }
        
    }
}
