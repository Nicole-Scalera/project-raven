using UnityEngine;

// This class is responsible for creating and opening a popup of the
// given prefab and adding it to the UI canvas of the current scene.
public class PopupOpener : MonoBehaviour
{
    public GameObject popupPrefab; // Prefab
    protected Canvas m_canvas; // Canvas Host
    protected GameObject m_popup; // Popup that contains the prefab

    protected void Start()
    {
        m_canvas = GetComponentInParent<Canvas>();
    }

    public virtual void OpenPopup()
    {
        m_popup = Instantiate(popupPrefab, m_canvas.transform, false);
        m_popup.SetActive(true);
        // m_popup.GetComponent<Popup>().Open();
    }
}