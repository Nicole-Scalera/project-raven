using UnityEngine;

/*
 * 
 * Variables:
 * - boxPrefabs: contains the prefabs for each box
 * - timer: manual timer for spawning boxes
 * - maxTime: max amount of time for the timer in seconds
 * 
 */

namespace ConveyorBelt_cf
{
    public class ObjectManager : MonoBehaviour
    {

        private Vector3 spawnPosition;

        [Header("Prefabs")] public GameObject[] boxPrefabs;

        [Header("Box Spawning")] public float timer = 0;
        public float maxTime = 5;

        [Header("Pathing")] public GameObject path;

        private void Awake()
        {

            spawnPosition = transform.position;

        }

        private void Start()
        {

            Debug.Log(boxPrefabs.Length);

        }

        private void Update()
        {
            // if the timer is not at its maxTime increment the timer
            // else create a new box and reset the timer
            if (timer < maxTime)
            {

                timer += Time.deltaTime;

            }
            else
            {

                //TODO: figure out a way to add a tag to the instantiate

                GameObject newBox = Instantiate(boxPrefabs[Random.Range(0,boxPrefabs.Length)], spawnPosition, Quaternion.identity);
                //GameObject newBox = Instantiate(boxPrefabs[2], spawnPosition, Quaternion.identity);

                if (newBox.name.Contains("Standard Box"))
                {

                    newBox.transform.localScale = new(100f, 100f, 100f);

                }
                else if (newBox.name.Contains("Square Box"))
                {

                    newBox.transform.localScale = new(50f, 50f, 50f);

                }
                else if (newBox.name.Contains("Bloated Box"))
                {

                    newBox.transform.localScale = new(1.5f, 1.5f, 1.5f);                    

                }

                newBox.AddComponent<BoxCollider>();

                newBox.AddComponent<Rigidbody>();
                newBox.GetComponent<Rigidbody>().useGravity = true;

                newBox.AddComponent<InteractableBox>();

                path.GetComponent<BeltBehavior>().AddBox(newBox);

                timer = 0;

            }

        }

    }
}