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

namespace ConveyorBelt_cf
{
    public class BeltBehavior : MonoBehaviour
    {

        public List<GameObject> boxes = new List<GameObject>();

        public Node[] pathNodes;

        public float moveSpeed = 1f;

        public Vector3 currentPosition;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

            //Fills the array with the current node children of the path
            pathNodes = GetComponentsInChildren<Node>();

            for (int i = 0; i < pathNodes.Length; i++)
            {

                Debug.Log(pathNodes[i].name + " Global Pos: " + pathNodes[i].transform.position);
                Debug.Log(pathNodes[i].name + " Local Pos: " + pathNodes[i].transform.localPosition);

            }

        }


        public Vector3 NextPosition(int currentNodePosition)
        {
            if (currentNodePosition < pathNodes.Length - 1)
            {
                return pathNodes[currentNodePosition].transform.position;
            }
            else
            {
                return pathNodes[pathNodes.Length - 1].transform.position;
            }
            
        }

        //Takes in a gameobject and adds it to the boxes array
        public void AddBox(GameObject newBox)
        {

            boxes.Add(newBox);
            Debug.Log("Added Box Global: " + newBox.transform.position);
            Debug.Log("Added Box Local: " + newBox.transform.localPosition);

        }

        //Takes in a box gameobject and removes it from the list
        public void RemoveBox(GameObject newBox)
        {

            boxes.Remove(newBox);

        }

    }

}