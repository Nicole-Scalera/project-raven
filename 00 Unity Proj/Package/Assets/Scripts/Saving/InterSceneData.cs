using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class InterSceneData : MonoBehaviour
{

    public List<string> clues = new List<string>();

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        


    }

}
