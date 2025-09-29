using UnityEngine;

public class InteractableBox : MonoBehaviour
{

    public bool interactedWith = false;

    public int sortNum;

    public GameObject player;


    private void Start()
    {
    
        sortNum = Random.Range(1,2);

    }

    private void FixedUpdate()
    {

        if (interactedWith)
        {

            //Debug.Log("Moving Box");
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 2, player.transform.position.z);

        }
        

    }

    public void Interaction()
    {

        //Debug.Log(interactedWith);
        interactedWith = !interactedWith;

    }

}
