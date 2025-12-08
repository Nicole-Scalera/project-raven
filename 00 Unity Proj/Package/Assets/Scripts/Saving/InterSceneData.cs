using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterSceneData : MonoBehaviour
{

    public HashSet<string> clues = new HashSet<string>();
    public List<string> cluesList = new List<string>();
    public HashSet<GameObject> clueObjects = new HashSet<GameObject>();

    public bool factoryCompleted = false;
    public int factoryDay = 0;

    private void Awake()
    {

        DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {

        for (int i = 0; i < cluesList.Count; i++)
        {

            GameObject.Find(cluesList[i] + " UI").SetActive(true);

        }

    }

    public void AddClue(InteractableClue newClue)
    {

        clues.Add(newClue.name);
        clueObjects.Add(newClue.gameObject);
        cluesList = clues.ToList();

    }


}
