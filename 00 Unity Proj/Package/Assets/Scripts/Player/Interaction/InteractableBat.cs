using ConveyorBelt_cf;
using TMPro;
using UnityEngine;

public class InteractableBat : MonoBehaviour, IInteractable
{

    public bool interactedWith = false;
    public bool onWall = false;
    public Vector3 wall;

    public GameObject player;
    public Ray playerRay;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        onWall = true;
        wall = transform.position;

    }

    private void FixedUpdate()
    {

        if (interactedWith)
        {
                
            GetComponent<Rigidbody>().useGravity = false;
            Use();

        }
        else if(onWall)
        {

            GetComponent<Rigidbody>().useGravity = false;

        }
        else
        {

            GetComponent<Rigidbody>().useGravity = true;

        }

    }

    public void Interaction()
    {

        interactedWith = !interactedWith;

        if (!interactedWith && (this.transform.position.x == wall.x && this.transform.position.z == wall.z))
        {

            onWall = true;

        }

    }

    public void Use()
    {

        transform.rotation = Quaternion.identity;

        playerRay = player.GetComponent<PlayerRaycastInteraction>().interactionRay;
        transform.position = new Vector3(playerRay.GetPoint(1f).x - 0.25f, playerRay.GetPoint(1f).y, playerRay.GetPoint(1f).z);

    }

}
