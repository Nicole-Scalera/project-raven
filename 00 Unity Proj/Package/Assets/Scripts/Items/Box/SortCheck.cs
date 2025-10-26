using TMPro;
using UnityEngine;

public class SortCheck : MonoBehaviour
{

    [Header("Sorting Number")]
    public int sortNum;

    public int sortedBoxes;
    public int totalBoxes;

    public TextMeshProUGUI quotaUI;

    public Vector3 sortedPosition;

    private void Start()
    {

        sortedBoxes = 0;
        totalBoxes = 2;

    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collider Entered");

        if (other.gameObject.GetComponent<InteractableBox>() != null)
        {

            GameObject activeBox = other.gameObject;

            if (sortNum == activeBox.GetComponent<InteractableBox>().sortNum && !activeBox.GetComponent <InteractableBox>().sorted)
            {

                Debug.Log("Sorted Correctly");
                other.gameObject.GetComponent<InteractableBox>().interactedWith = false;
                other.gameObject.GetComponent<InteractableBox>().sorted = true;
                other.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                other.gameObject.transform.position = sortedPosition;
                sortedBoxes += 1;
                quotaUI.text = sortedBoxes.ToString() + " / " + totalBoxes.ToString();

            }
            else
            {

                Debug.Log("Sorted Incorrectly");

            }

        }

    }

}
