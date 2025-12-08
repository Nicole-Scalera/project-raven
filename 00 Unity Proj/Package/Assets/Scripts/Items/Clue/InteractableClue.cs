using System;
using UnityEngine;

public class InteractableClue : MonoBehaviour, IInteractable
{
    public string clueName;
    public int clueID;
    public GameObject uiPanel;
    public bool interactedWith = false;

    public GameObject global;

    private void Start()
    {

        global = GameObject.FindGameObjectWithTag("Global");

    }

    public void Interaction()
    {

        interactedWith = true;
        uiPanel.SetActive(true);
        global.GetComponent<InterSceneData>().AddClue(this);

    }

}
