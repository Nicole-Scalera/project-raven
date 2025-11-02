using System.Runtime.CompilerServices;
using ConveyorBelt_cf;
using NUnit.Framework;
using TMPro;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;

public class BeatenBox : MonoBehaviour
{
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
        if (!player.GetComponent<PlayerRaycastInteraction>().hasBat)
        {

            interactedWith = !interactedWith;

            sortedBayUI.text = "Bay: " + sortTruck.ToString() + "\n" + "Shelf: " + sortShelf + "\n" + "Spot: " + sortSpot.ToString();

            path.GetComponent<BeltBehavior>().RemoveBox(this.gameObject);

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
}
