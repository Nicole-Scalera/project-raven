using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InterSceneData : MonoBehaviour
{

    public List<InteractableClue> clues = new List<InteractableClue>();

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

    }

    public void AddClue(InteractableClue newClue)
    {

        clues.Add(newClue);

    }


}
