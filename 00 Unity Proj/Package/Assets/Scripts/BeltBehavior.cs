using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

/*
 * 
 * Variables:
 * - box: contains the current gameobject for a stand in box
 * - pathNodes: contains an array of nodes for the box to follow
 * - moveSpeed: how fast the box moves along the path
 * - currentPosition: the position of the currentNode
 * - currentNode: the currently accessed node in the pathNodes array
 * - boxSpeed: overall speed of the box while following the path
 * 
 */

public class BeltBehavior : MonoBehaviour
{

    public List<GameObject> boxes = new List<GameObject>();

    public Node[] pathNodes;

    public float moveSpeed = 10f;

    static Vector3 currentPosition;

    private int currentNode;

    private float boxSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Fills the array with the current node children of the path
        pathNodes = GetComponentsInChildren<Node>();

        CheckNode();

    }

    void CheckNode()
    {

        //reset speed temporarily to 0 and assign new node position to current position
        boxSpeed = 0;
        currentPosition = pathNodes[currentNode].transform.position;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        foreach(GameObject box in boxes)
        {

            //boxSpeed is assigned to the time between frame updates and moveSpeed
            boxSpeed = Time.deltaTime * moveSpeed;

            //if the box position is not at the current goal node position the box moves closer towards the goal node position
            //if it is the current goal node position the currentNode variable increments and CheckNode() gets called to update
            //the next goal node position

            if (box.transform.position != currentPosition)
            {

                box.transform.position = Vector3.MoveTowards(box.transform.position, currentPosition, boxSpeed);

            }
            else
            {
                if (currentNode < pathNodes.Length - 1)
                {

                    currentNode++;
                    CheckNode();

                }

            }

        }

    }


    //Takes in a gameobject and adds it to the boxes array
    // TODO: Check for tag to confirm its a box
    public void AddBox(GameObject newBox)
    {

        boxes.Add(newBox);

        Debug.Log("New Box Added: " + newBox.name);

    }

}
