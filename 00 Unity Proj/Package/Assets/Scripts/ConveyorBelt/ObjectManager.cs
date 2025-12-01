using System;
using System.Collections.Generic;
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
        private int interactableLayer = 7;

        [Header("Prefabs")] 
        public GameObject[] boxPrefabs;

        [Header("Box Spawning")] 
        public float timer = 0;
        public float maxTime = 5;

        [Header("Pathing")] 
        public GameObject path;

        [Header("Sorting")]
        string sortingPosition;
        int maxTruckBoxes = 30;
        public List<string> truckOneBoxes = new List<string>();
        public List<string> truckTwoBoxes = new List<string>();
        public List<string> truckThreeBoxes = new List<string>();
        public List<string> truckFourBoxes = new List<string>();
        public List<string> truckFiveBoxes = new List<string>();

        private void Awake()
        {

            spawnPosition = transform.position;

            for (int i = 1; i < maxTruckBoxes + 1; i++)
            { 

                if (i < 11)
                {
                    truckOneBoxes.Add("1Top" + (i%10).ToString());
                    truckTwoBoxes.Add("2Top" + (i % 10).ToString());
                    truckThreeBoxes.Add("3Top" + (i % 10).ToString());
                    truckFourBoxes.Add("4Top" + (i % 10).ToString());
                    truckFiveBoxes.Add("5Top" + (i % 10).ToString());
                }
                else if (i < 21)
                {
                    truckOneBoxes.Add("1Mid" + (i % 10).ToString());
                    truckTwoBoxes.Add("2Mid" + (i % 10).ToString());
                    truckThreeBoxes.Add("3Mid" + (i % 10).ToString());
                    truckFourBoxes.Add("4Mid" + (i % 10).ToString());
                    truckFiveBoxes.Add("5Mid" + (i % 10).ToString());
                }
                else
                {
                    truckOneBoxes.Add("1Bot" + (i % 10).ToString());
                    truckTwoBoxes.Add("2Bot" + (i % 10).ToString());
                    truckThreeBoxes.Add("3Bot" + (i % 10).ToString());
                    truckFourBoxes.Add("4Bot" + (i % 10).ToString());
                    truckFiveBoxes.Add("5Bot" + (i % 10).ToString());
                }
                

            }

        }

        private void FixedUpdate()
        {
            // if the timer is not at its maxTime increment the timer
            // else create a new box and reset the timer
            if (timer < maxTime)
            {

                timer += Time.deltaTime;

            }
            else
            {

                CreateBox();
                timer = 0;

            }

        }

        private void CreateBox()
        {

            int randomTruck = UnityEngine.Random.Range(1, 6);

            if (randomTruck == 1)
            {
                sortingPosition = truckOneBoxes[UnityEngine.Random.Range(0, truckOneBoxes.Count)];
                truckOneBoxes.Remove(sortingPosition);
            }
            else if (randomTruck == 2)
            {
                sortingPosition = truckTwoBoxes[UnityEngine.Random.Range(0, truckTwoBoxes.Count)];
                truckTwoBoxes.Remove(sortingPosition);
            }
            else if (randomTruck == 3)
            {
                sortingPosition = truckThreeBoxes[UnityEngine.Random.Range(0, truckThreeBoxes.Count)];
                truckThreeBoxes.Remove(sortingPosition);
            }
            else if (randomTruck == 4)
            {
                sortingPosition = truckFourBoxes[UnityEngine.Random.Range(0, truckFourBoxes.Count)];
                truckFourBoxes.Remove(sortingPosition);
            }
            else if (randomTruck == 5)
            {
                sortingPosition = truckFiveBoxes[UnityEngine.Random.Range(0, truckFiveBoxes.Count)];
                truckFiveBoxes.Remove(sortingPosition);
            }

            GameObject newBox = Instantiate(boxPrefabs[UnityEngine.Random.Range(0, boxPrefabs.Length)], spawnPosition, Quaternion.identity);

            if (newBox.name.Contains("Standard Box"))
            {

                newBox.transform.localScale = new(100f, 100f, 100f);
                newBox.AddComponent<InteractableBox>().sortedPosition = sortingPosition;

            }
            else if (newBox.name.Contains("Square Box"))
            {

                newBox.transform.localScale = new(50f, 50f, 50f);
                newBox.AddComponent<TapedBox>().sortedPosition = sortingPosition;

            }
            else if (newBox.name.Contains("Bloated Box"))
            {

                newBox.transform.localScale = new(3.5f, 3.5f, 3f);
                newBox.AddComponent<BeatenBox>().sortedPosition = sortingPosition;

            }

            newBox.AddComponent<BoxCollider>();

            newBox.AddComponent<Rigidbody>();
            newBox.GetComponent<Rigidbody>().useGravity = true;
            newBox.GetComponent<Rigidbody>().angularDamping = 10;

            path.GetComponent<BeltBehavior>().AddBox(newBox);

        }

    }
}