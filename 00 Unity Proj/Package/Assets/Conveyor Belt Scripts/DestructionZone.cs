using Unity.VisualScripting;
using UnityEngine;

public class DestructionZone : MonoBehaviour
{

    public GameObject path;

    //When an collider hits the destruction zone it removes the gameobjct
    //from the boxes array and then destroys the object
    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Entered: " +  collision.gameObject.name);

        path.GetComponent<BeltBehavior>().RemoveBox(collision.gameObject);

        Destroy(collision.gameObject);
        
    }

}
