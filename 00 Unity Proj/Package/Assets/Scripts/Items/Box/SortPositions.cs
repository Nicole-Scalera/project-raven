using System.Collections.Generic;
using UnityEngine;

public class SortPositions : MonoBehaviour
{

    public int numTrucks = 5;
    public List<Vector3> truckOneSortedPositions;
    public List<Vector3> truckTwoSortedPositions;
    public List<Vector3> truckThreeSortedPositions;
    public List<Vector3> truckFourSortedPositions;
    public List<Vector3> truckFiveSortedPositions;

    private void Start()
    {

        AddPositions();      
        //SpawnBoxes();

    }
    private void AddPositions()
    {

        for (int i = 1; i < numTrucks + 1; i++) //number of trucks
        {

            for (int j = 0; j < 6; j++) //3 is the number of shelves per side of the truck
            {

                for (int k = 0; k < 5; k++) //5 is the number of boxes per shelf
                {

                    /*
                     * checks truck number (i)
                     * adjusts by where on shelf by (k)
                     * adjusts by shelf (Top (%0), Mid (%1), Bot (%2)) by (j % 3)
                     * adjusts by shelf (Left (%1), Right(%0)) (j % 2)
                     * 
                     * Box Numbers Per Shelf: 0 -> 9 (0 -> 4 Left | 5 -> 9 Right Side)
                     * Order(index): Top Right(0-4), Middle Left(5-9), Bottom Right(10-14), Top Left(15-19), Middle Right(20-24), Bottom Left (25-29)
                     * 
                     */

                    if (i == 1)
                    {
                        truckOneSortedPositions.Add(new Vector3(-8.885f + (k * 1.1223f), 4.181f - ((j % 3) * 1.0375f), 16.859f + ((j % 2) * 2.386f)));
                    }
                    else if (i == 2)
                    {
                        truckTwoSortedPositions.Add(new Vector3(-8.885f + (k * 1.1223f), 4.181f - (j % 3 * 1.0375f), 12.71f + ((j % 2) * 2.386f)));
                    }
                    else if (i == 3)
                    {
                        truckThreeSortedPositions.Add(new Vector3(-8.885f + (k * 1.1223f), 4.181f - (j % 3 * 1.0375f), 8.848f + ((j % 2) * 2.386f)));
                    }
                    else if (i == 4)
                    {
                        truckFourSortedPositions.Add(new Vector3(-8.885f + (k * 1.1223f), 4.181f - (j % 3 * 1.0375f), 4.88f + ((j % 2) * 2.386f)));
                    }
                    else if (i == 5)
                    {
                        truckFiveSortedPositions.Add(new Vector3(-8.885f + (k * 1.1223f), 4.181f - (j % 3 * 1.0375f), 0.714f + ((j % 2) * 2.386f)));
                    }

                }

            }

        }

    }

    private void SpawnBoxes()
    {
        
        for (int i = 0; i < truckOneSortedPositions.Count; i++)
        {

            GameObject newBox = Instantiate(GameObject.Find("Standard Box"));
            newBox.transform.position = truckOneSortedPositions[i];
            newBox.transform.rotation = Quaternion.identity;
            newBox.transform.localScale = new(100f, 100f, 100f);

        }
        for (int i = 0; i < truckTwoSortedPositions.Count; i++)
        {

            GameObject newBox = Instantiate(GameObject.Find("Standard Box"));
            newBox.transform.position = truckTwoSortedPositions[i];
            newBox.transform.rotation = Quaternion.identity;
            newBox.transform.localScale = new(100f, 100f, 100f);

        }
        for (int i = 0; i < truckThreeSortedPositions.Count; i++)
        {

            GameObject newBox = Instantiate(GameObject.Find("Standard Box"));
            newBox.transform.position = truckThreeSortedPositions[i];
            newBox.transform.rotation = Quaternion.identity;
            newBox.transform.localScale = new(100f, 100f, 100f);

        }
        for (int i = 0; i < truckFourSortedPositions.Count; i++)
        {

            GameObject newBox = Instantiate(GameObject.Find("Standard Box"));
            newBox.transform.position = truckFourSortedPositions[i];
            newBox.transform.rotation = Quaternion.identity;
            newBox.transform.localScale = new(100f, 100f, 100f);

        }
        for (int i = 0; i < truckFiveSortedPositions.Count; i++)
        {
            Debug.Log(truckFiveSortedPositions[i]);
            GameObject newBox = Instantiate(GameObject.Find("Standard Box"));
            newBox.transform.position = truckFiveSortedPositions[i];
            newBox.transform.rotation = Quaternion.identity;
            newBox.transform.localScale = new(100f, 100f, 100f);

        }

    }

}
