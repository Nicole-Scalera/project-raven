using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private PauseMenu pauseMenu;
    private GameObject pauseMenuUI;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get a reference to this game object
        pauseMenuUI = this.gameObject;
    }
}
