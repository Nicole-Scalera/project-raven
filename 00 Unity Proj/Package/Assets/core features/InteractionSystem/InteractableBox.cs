using ConveyorBelt_cf;
using TMPro;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{

    public bool interactedWith = false;

    public int sortNum;

    public bool sorted = false;

    public GameObject player;
    public Ray playerRay;

    public GameObject path;

    public TextMeshProUGUI sortedBayUI;


    private void Start()
    {
    
        sortNum = Random.Range(1,5);

        player = GameObject.FindGameObjectWithTag("Player");
        //playerRay = player.GetComponent<PlayerRaycastInteraction>().interactionRay;
        
        path = GameObject.FindGameObjectWithTag("Path");

        GameObject quotaUI = GameObject.FindGameObjectWithTag("Game UI");
        sortedBayUI = quotaUI.GetComponent<TextMeshProUGUI>();

    }

    private void FixedUpdate()
    {

        if (interactedWith)
        {
            playerRay = player.GetComponent<PlayerRaycastInteraction>().interactionRay;
            transform.position = playerRay.GetPoint(1.5f);
            transform.rotation = player.transform.rotation;


        }

        if (sorted)
        {

            transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().isKinematic = true;

        }
        

    }

    public void Interaction()
    {

        interactedWith = !interactedWith;

        sortedBayUI.text = "Bay: " + sortNum.ToString();

        path.GetComponent<BeltBehavior>().RemoveBox(this.gameObject);

        GetComponent<Rigidbody>().useGravity = false;

    }

}
