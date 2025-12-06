using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableApartmentDoor : MonoBehaviour, IInteractable
{
    public void Interaction()
    {

        SceneManager.LoadScene("Test_Factory");

    }
}
