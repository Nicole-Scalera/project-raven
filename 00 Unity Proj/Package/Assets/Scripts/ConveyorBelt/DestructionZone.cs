using Unity.VisualScripting;
using UnityEngine;
using SceneSwitching_cf;

namespace ConveyorBelt_cf
{
    public class DestructionZone : MonoBehaviour
    {

        public GameObject path;
        public GameObject objectManager;

        //When an collider hits the destruction zone it removes the gameobjct
        //from the boxes array and then destroys the object
        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("Entered: " + other.gameObject.name);

            if (other.gameObject.GetComponent<InteractableBox>() != null)
            {

                path.GetComponent<BeltBehavior>().RemoveBox(other.gameObject);

                if (other.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "1")
                {
                    objectManager.GetComponent<ObjectManager>().truckOneBoxes.Add(other.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "2")
                {
                    objectManager.GetComponent<ObjectManager>().truckTwoBoxes.Add(other.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "3")
                {
                    objectManager.GetComponent<ObjectManager>().truckThreeBoxes.Add(other.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "4")
                {
                    objectManager.GetComponent<ObjectManager>().truckFourBoxes.Add(other.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "5")
                {
                    objectManager.GetComponent<ObjectManager>().truckFiveBoxes.Add(other.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }

                Destroy(other.gameObject);

            }
            else if (other.gameObject.GetComponent<BeatenBox>() != null)
            {

                path.GetComponent<BeltBehavior>().RemoveBox(other.gameObject);

                if (other.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "1")
                {
                    objectManager.GetComponent<ObjectManager>().truckOneBoxes.Add(other.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "2")
                {
                    objectManager.GetComponent<ObjectManager>().truckTwoBoxes.Add(other.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "3")
                {
                    objectManager.GetComponent<ObjectManager>().truckThreeBoxes.Add(other.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "4")
                {
                    objectManager.GetComponent<ObjectManager>().truckFourBoxes.Add(other.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (other.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "5")
                {
                    objectManager.GetComponent<ObjectManager>().truckFiveBoxes.Add(other.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }

                Destroy(other.gameObject);

            }
        }
    }

    
    }