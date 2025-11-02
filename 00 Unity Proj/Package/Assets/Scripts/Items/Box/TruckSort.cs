using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using UnityEngine;

public class TruckSort : MonoBehaviour
{
    [Header("Sorting Number")]
    public int truckNum;
    public List<Vector3> positions;

    private SortPositions sortPositions;

    public int sortedBoxes = 0;

    //Gets the correct truck position from the parent script
    private void Awake()
    {

        sortPositions = GetComponentInParent<SortPositions>();

        if (truckNum == 1)
        {
            positions = sortPositions.truckOneSortedPositions;
        }
        if (truckNum == 2)
        {
            positions = sortPositions.truckTwoSortedPositions;
        }
        if (truckNum == 3)
        {
            positions = sortPositions.truckThreeSortedPositions;
        }
        if (truckNum == 4)
        {
            positions = sortPositions.truckFourSortedPositions;
        }
        if (truckNum == 5)
        {
            positions = sortPositions.truckFiveSortedPositions;
        }

    }

    //When a collider overlaps check if its a box and in the right area then sort accordingly
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("Collider Entered");

        if (other.GetComponent<InteractableBox>() != null)
        {

            InteractableBox box = other.GetComponent<InteractableBox>();

            if (!box.sorted && box.sortTruck == truckNum)
            {

                if (box.sortShelf == "Top")
                {

                    if (box.sortSpot < 5)
                    {
                        box.gameObject.transform.position = positions[box.sortSpot]; //Left Boxes
                    }
                    else
                    {
                        box.gameObject.transform.position = positions[(box.sortSpot + 10)]; //Right Boxes
                    }

                }
                else if (box.sortShelf == "Mid")
                {

                    if (box.sortSpot < 5)
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 5]; //Left Boxes
                    }
                    else
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 15]; //Right Boxes
                    }

                }
                else if (box.sortShelf == "Bot")
                {

                    if (box.sortSpot < 5)
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 10]; //Left Boxes
                    }
                    else
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 20]; //Right Boxes
                    }

                }

                sortedBoxes += 1;
                GetComponentInParent<SortManager>().totalSorted += 1;

                box.gameObject.GetComponent<InteractableBox>().interactedWith = false;
                box.gameObject.GetComponent<InteractableBox>().sorted = true;
            }

        }
        else if (other.GetComponent<BeatenBox>() != null)
        {

            BeatenBox box = other.GetComponent<BeatenBox>();

            if (box.sortable && !box.sorted && box.sortTruck == truckNum)
            {

                if (box.sortShelf == "Top")
                {

                    if (box.sortSpot < 5)
                    {
                        box.gameObject.transform.position = positions[box.sortSpot]; //Left Boxes
                    }
                    else
                    {
                        box.gameObject.transform.position = positions[(box.sortSpot + 10)]; //Right Boxes
                    }

                }
                else if (box.sortShelf == "Mid")
                {

                    if (box.sortSpot < 5)
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 5]; //Left Boxes
                    }
                    else
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 15]; //Right Boxes
                    }

                }
                else if (box.sortShelf == "Bot")
                {

                    if (box.sortSpot < 5)
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 10]; //Left Boxes
                    }
                    else
                    {
                        box.gameObject.transform.position = positions[box.sortSpot + 20]; //Right Boxes
                    }

                }

                sortedBoxes += 1;
                GetComponentInParent<SortManager>().totalSorted += 1;
                box.gameObject.GetComponent<BeatenBox>().interactedWith = false;
                box.gameObject.GetComponent<BeatenBox>().sorted = true;
            }

        }

    }
}
