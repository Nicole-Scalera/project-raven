using BasicMovement2_cf;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// This class is responsible for Pause Menu management. Popups follow the traditional behavior of
// automatically blocking the input on elements behind it and adding a background texture.
public class PauseManager : MonoBehaviour, PlayerControls.IGameControlsActions
{
    public GameObject popupPrefab;
    private PlayerControls playerControls; // PlayerControls.cs
    protected Canvas m_canvas;
    protected GameObject m_popup;
    private bool isPaused;
    
    private void Awake()
    {
        // Initialize PlayerControls
        // playerControls = new PlayerControls();
        // playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        // playerControls.GameControls.Enable();
    }
    
    private void OnEnable()
    {
        SceneManager.sceneLoaded += GetPlayerControls;
        GameStateManager.gameStateChanged += SetPauseState;

        // Initialize the local pause state from the static current state in case
        // this component is enabled after the GameStateManager has already set it.
        // This ensures PauseManager reflects the current state even when it's on a
        // different GameObject.
        SetPauseState(GameStateManager.CurrentGameState);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= GetPlayerControls;
        GameStateManager.gameStateChanged -= SetPauseState;
    }
    
    private void SetPauseState(GameStateManager.GameState newState)
    {
        if (newState == GameStateManager.GameState.Paused)
        {
            isPaused = true;
        }
        else
        {
            isPaused = false;
        }
    }
    
    private void GetPlayerControls(Scene scene, LoadSceneMode sceneMode)
    {
        playerControls = new PlayerControls();
        playerControls.GameControls.SetCallbacks(this); // Set this class as listener
        playerControls.GameControls.Enable();
    }

    protected void Start()
    {
        m_canvas = GetComponentInParent<Canvas>();

        // Fallback: if the PauseManager isn't placed under a Canvas in the hierarchy,
        // try to find any active Canvas in the scene so popups can be parented and shown.
        if (m_canvas == null)
        {
            m_canvas = FindObjectOfType<Canvas>();
        }

        // Ensure local pause state reflects the current global state (extra safety).
        SetPauseState(GameStateManager.CurrentGameState);
    }

    public virtual void OpenPopup()
    {
        Debug.Log("Opening popup");
        m_popup = Instantiate(popupPrefab);
        m_popup.SetActive(true);
    }

    // // If we have a canvas, parent the popup to it and make sure the RectTransform
    // // stretches to cover the canvas (useful for Screen Space - Overlay prefabs).
    // if (m_canvas != null && m_popup != null)
    // {
    //     m_popup.transform.SetParent(m_canvas.transform, false);
    //
    //     RectTransform rt = m_popup.GetComponent<RectTransform>();
    //     if (rt != null)
    //     {
    //         rt.anchorMin = Vector2.zero;
    //         rt.anchorMax = Vector2.one;
    //         rt.anchoredPosition = Vector2.zero;
    //         rt.sizeDelta = Vector2.zero;
    //     }
    // }
    
    
    public void OnTogglePause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // If the game is Playing
        if (!isPaused)
        {
            if (m_popup == null)
            {
                OpenPopup();
                GameStateManager.SetGameState(GameStateManager.GameState.Paused);
            }
        }
        // If the game is Paused
        else if (isPaused)
        {
            if (m_popup != null)
            {
                m_popup.SetActive(false);
                Destroy(m_popup);
                // Resume the game state
                GameStateManager.SetGameState(GameStateManager.GameState.Playing);
            }
        }
    }
}