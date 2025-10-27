using ConveyorBelt_cf;
using TMPro;
using UnityEngine;

public class InteractableBat : MonoBehaviour
{

    public bool interactedWith = false;

    public GameObject player;
    public Ray playerRay;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void FixedUpdate()
    {

        if (interactedWith)
        {
                
            GetComponent<Rigidbody>().useGravity = false;
            Use();

        }
        else
        {

            GetComponent<Rigidbody>().useGravity = true;

        }

    }

    public void Interaction()
    {

        interactedWith = !interactedWith;

    }

    public void Use()
    {

        transform.rotation = Quaternion.identity;

        playerRay = player.GetComponent<PlayerRaycastInteraction>().interactionRay;
        transform.position = new Vector3(playerRay.GetPoint(1f).x - 0.25f, playerRay.GetPoint(1f).y, playerRay.GetPoint(1f).z);

    }

}
