using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SortManager : MonoBehaviour
{

    public int totalSorted = 0;
    int totalNeeded = 15;

    public TextMeshProUGUI quotaUI;
    public TextMeshProUGUI bayUI;

    public GameObject frontDoor;
    public GameObject global;

    private void Start()
    {

        frontDoor = GameObject.FindGameObjectWithTag("Door");
        global = GameObject.FindGameObjectWithTag("Global");

    }

    private void Update()
    { 

        quotaUI.text = totalSorted.ToString() + "/" + totalNeeded.ToString();

        if (!frontDoor.GetComponent<InteractableFactoryDoor>().unlocked && totalSorted >= totalNeeded)
        {

            frontDoor.GetComponent<InteractableFactoryDoor>().unlocked = true;
            global.GetComponent<InterSceneData>().factoryCompleted = true;


        }

    }

}
