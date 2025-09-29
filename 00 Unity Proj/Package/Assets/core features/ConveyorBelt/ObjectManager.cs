using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityCommunity.UnitySingleton;
using UnityEngine.UIElements;

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

                GameObject newBox = Instantiate(boxPrefabs[0], spawnPosition, Quaternion.identity);

                path.GetComponent<BeltBehavior>().AddBox(newBox);

                timer = 0;

            }

        }

    }
}