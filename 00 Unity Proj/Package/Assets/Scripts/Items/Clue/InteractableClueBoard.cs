using System.Collections.Generic;
using UnityEngine;

public class InteractableClueBoard : MonoBehaviour, IInteractable
{

    public GameObject gameUI;
    public GameObject clueBoardUI;

    private void Start()
    {

        gameUI = GameObject.FindGameObjectWithTag("UI");

    }


    public void Interaction()
    {
        gameUI.SetActive(false);
        clueBoardUI.SetActive(true);
    }
}
