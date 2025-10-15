using ConveyorBelt_cf;
using TMPro;
using UnityEngine;

public class InteractableBox : MonoBehaviour
{

    public bool interactedWith = false;

    public int sortNum;

    public bool sorted = false;

    public GameObject player;

    public GameObject path;

    public TextMeshProUGUI sortedBayUI;


    private void Start()
    {
    
        sortNum = Random.Range(1,5);
        player = GameObject.FindGameObjectWithTag("Player");
        path = GameObject.FindGameObjectWithTag("Path");

        GameObject quotaUI = GameObject.FindGameObjectWithTag("Game UI");
        //sortedBayUI = quotaUI.GetComponent<TextMeshProUGUI>();

    }

    private void FixedUpdate()
    {

        if (interactedWith)
        {

            //Debug.Log("Moving Box");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);

        }

        if (sorted)
        {

            GetComponent<Rigidbody>().isKinematic = true;

        }
        

    }

    public void Interaction()
    {

        //Debug.Log(interactedWith);
        interactedWith = !interactedWith;

        sortedBayUI.text = "Bay: " + sortNum.ToString();

        path.GetComponent<BeltBehavior>().RemoveBox(this.gameObject);

        transform.rotation = Quaternion.identity;


    }

}
