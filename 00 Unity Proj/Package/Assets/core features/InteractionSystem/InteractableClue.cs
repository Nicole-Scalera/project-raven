using UnityEngine;

public class InteractableClue : MonoBehaviour
{

    public string clueName;
    public bool interactedWith = false;
    
    public string InteractedWith()
    {

        interactedWith = true;
        return clueName;

    }

}
