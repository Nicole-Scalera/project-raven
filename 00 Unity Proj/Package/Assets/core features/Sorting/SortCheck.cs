using UnityEngine;

public class SortCheck : MonoBehaviour
{

    [Header("Sorting Number")]
    public int sortNum;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collider Entered");

        if (other.gameObject.GetComponent<InteractableBox>() != null)
        {

            if (sortNum == other.gameObject.GetComponent<InteractableBox>().sortNum)
            {

                Debug.Log("Sorted Correctly");
                other.gameObject.GetComponent<InteractableBox>().interactedWith = false;
                other.gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                other.gameObject.transform.position = new Vector3(7.5f, 1.5f, -8.8f);

            }
            else
            {

                Debug.Log("Sorted Incorrectly");

            }

        }

    }

}
