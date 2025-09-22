using UnityEngine;

public class InteractableBox : MonoBehaviour
{

    public bool interactedWith = false;
    public float distanceX;
    public float distanceY;
    public float distanceZ;

    public GameObject player;

    private void FixedUpdate()
    {

        if (interactedWith)
        {

            Debug.Log("Moving Box");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);

        }
        

    }

    public void Interaction()
    {

        Debug.Log(interactedWith);
        interactedWith = !interactedWith;

    }

}
