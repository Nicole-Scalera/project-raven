using TMPro;
using UnityEngine;

public class ClueBoardUI : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject clueBoardUI;
    public TextMeshProUGUI text;
    public float timer = 0.0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    private void FixedUpdate()
    {

        if (timer < 2f)
        {
            timer += Time.deltaTime;
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
    public void ChangeUI()
    {
        Debug.Log("Change UI");
        gameUI.SetActive(true);
        clueBoardUI.SetActive(false);
        Cursor.visible = false;
        
    }
}
