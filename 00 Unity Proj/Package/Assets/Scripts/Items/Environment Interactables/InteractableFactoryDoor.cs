using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableFactoryDoor : MonoBehaviour, IInteractable
{
    public bool unlocked = false;

    public void Interaction()
    {
        if (unlocked)
        {
            SceneManager.LoadScene("GS_Apartment");
        }

    }
}
