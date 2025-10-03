using Unity.VisualScripting;
using UnityEngine;
using SceneSwitching_cf;

namespace ConveyorBelt_cf
{
    public class DestructionZone : MonoBehaviour
    {

        public GameObject path;

        //When an collider hits the destruction zone it removes the gameobjct
        //from the boxes array and then destroys the object
        private void OnCollisionEnter(Collision collision)
        {

            Debug.Log("Entered: " + collision.gameObject.name);

            if (collision.gameObject.GetComponent<InteractableBox>() != null)
            {

                path.GetComponent<BeltBehavior>().RemoveBox(collision.gameObject);

                Destroy(collision.gameObject);

            }

        }
    }
}