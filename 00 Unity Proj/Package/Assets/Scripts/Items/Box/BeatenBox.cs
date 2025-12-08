using System.Runtime.CompilerServices;
using ConveyorBelt_cf;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class BeatenBox : MonoBehaviour, IInteractable
{

    public Vector3 goalPosition = Vector3.zero;
    public int goalPositionIndex = 0;
    private float boxSpeed = 1f;

    public bool interactedWith = false;

    public string sortedPosition;
    public int sortTruck;
    public string sortShelf;
    public int sortSpot;

    public bool sortable = false;
    public bool sorted = false;

    public GameObject player;
    public Ray playerRay;

    public GameObject path;

    public TextMeshProUGUI sortedBayUI;

    public Material beatenTexture;
    private Material[] newMaterials;

    private void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");

        path = GameObject.FindGameObjectWithTag("Path");

        GameObject quotaUI = GameObject.FindGameObjectWithTag("Game UI");
        sortedBayUI = quotaUI.GetComponent<TextMeshProUGUI>();

        beatenTexture = Resources.Load<Material>("phong1");

        sortTruck = int.Parse(sortedPosition.Substring(0, 1));
        sortShelf = sortedPosition.Substring(1, 3);
        sortSpot = int.Parse(sortedPosition.Substring(4, 1));

        goalPosition = path.GetComponent<BeltBehavior>().NextPosition(goalPositionIndex);

    }

    private void FixedUpdate()
    {

        if (this.interactedWith)
        {
            playerRay = player.GetComponent<PlayerRaycastInteraction>().interactionRay;
            transform.position = playerRay.GetPoint(1.5f);
            transform.rotation = player.transform.rotation;
            goalPosition = this.transform.position;
            path = null;

        }
        else if (this.sorted)
        {

            Quaternion sortedRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            transform.rotation = sortedRotation;
            GetComponent<Rigidbody>().isKinematic = true;

        }
        else
        {
            Vector3 roundedBoxPosition = RoundVector3(this.transform.position);
            Vector3 roundedGoalPosition = RoundVector3(goalPosition);

            if (roundedBoxPosition.x != roundedGoalPosition.x && roundedBoxPosition.z != roundedGoalPosition.z)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, goalPosition, boxSpeed * Time.deltaTime);
            }
            else
            {
                if (path != null)
                {
                    goalPosition = path.GetComponent<BeltBehavior>().NextPosition(goalPositionIndex);
                    goalPositionIndex += 1;
                }

            }

        }


    }

    public void Interaction()
    {
        if (!player.GetComponent<PlayerRaycastInteraction>().hasBat)
        {

            interactedWith = !interactedWith;

            sortedBayUI.text = "Bay: " + sortTruck.ToString() + "\n" + "Shelf: " + sortShelf + "\n" + "Spot: " + sortSpot.ToString();

            if (path != null)
            {
                path.GetComponent<BeltBehavior>().RemoveBox(this.gameObject);
            }

            GetComponent<Rigidbody>().useGravity = !interactedWith;

        }
        else
        {
            newMaterials = GetComponent<MeshRenderer>().materials;

            newMaterials[0] = beatenTexture;
            newMaterials[1] = beatenTexture;

            GetComponent<MeshRenderer>().materials = newMaterials;

            sortable = true;

        }
    }

    private Vector3 RoundVector3(Vector3 unroundedVector3)
    {

        float roundedX = Mathf.Round(unroundedVector3.x * 100);
        float roundedY = Mathf.Round(unroundedVector3.y * 100);
        float roundedZ = Mathf.Round(unroundedVector3.z * 100);

        return new Vector3(roundedX, roundedY, roundedZ);

    }


    
}
