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
        private void OnCollisionEnter(Collision collision)
        {

            Debug.Log("Entered: " + collision.gameObject.name);

            if (collision.gameObject.GetComponent<InteractableBox>() != null)
            {

                path.GetComponent<BeltBehavior>().RemoveBox(collision.gameObject);

                if (collision.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "1")
                {
                    objectManager.GetComponent<ObjectManager>().truckOneBoxes.Add(collision.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "2")
                {
                    objectManager.GetComponent<ObjectManager>().truckTwoBoxes.Add(collision.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "3")
                {
                    objectManager.GetComponent<ObjectManager>().truckThreeBoxes.Add(collision.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "4")
                {
                    objectManager.GetComponent<ObjectManager>().truckFourBoxes.Add(collision.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<InteractableBox>().sortedPosition.Substring(0, 1) == "5")
                {
                    objectManager.GetComponent<ObjectManager>().truckFiveBoxes.Add(collision.gameObject.GetComponent<InteractableBox>().sortedPosition);
                }

                Destroy(collision.gameObject);

            }
            else if(collision.gameObject.GetComponent<BeatenBox>() != null)
            {

                path.GetComponent<BeltBehavior>().RemoveBox(collision.gameObject);

                if (collision.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "1")
                {
                    objectManager.GetComponent<ObjectManager>().truckOneBoxes.Add(collision.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "2")
                {
                    objectManager.GetComponent<ObjectManager>().truckTwoBoxes.Add(collision.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "3")
                {
                    objectManager.GetComponent<ObjectManager>().truckThreeBoxes.Add(collision.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "4")
                {
                    objectManager.GetComponent<ObjectManager>().truckFourBoxes.Add(collision.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }
                if (collision.gameObject.GetComponent<BeatenBox>().sortedPosition.Substring(0, 1) == "5")
                {
                    objectManager.GetComponent<ObjectManager>().truckFiveBoxes.Add(collision.gameObject.GetComponent<BeatenBox>().sortedPosition);
                }

                Destroy(collision.gameObject);

            }

        }
    }
}