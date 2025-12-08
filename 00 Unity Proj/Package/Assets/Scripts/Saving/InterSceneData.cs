using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InterSceneData : MonoBehaviour
{

    public List<InteractableClue> clues = new List<InteractableClue>();

    public bool factoryCompleted = false;
    public int factoryDay = 0;

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

    }

    public void AddClue(InteractableClue newClue)
    {

        clues.Add(newClue);

    }


}
