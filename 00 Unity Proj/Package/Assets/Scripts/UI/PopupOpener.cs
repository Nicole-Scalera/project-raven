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
        // Get the canvas component
        m_canvas = GetComponentInParent<Canvas>();
    }

    public virtual void OpenPopup()
    {
        // Clone the popup prefab
        m_popup = Instantiate(popupPrefab, m_canvas.transform, false);
        m_popup.SetActive(true);
        m_popup.GetComponent<Popup>().Open();
        
        //=========================================================
        
        // m_popup = Instantiate(popupPrefab, m_canvas.transform, false);
        // m_popup.SetActive(true);
        //
        // var popupRect = m_popup.GetComponent<RectTransform>();
        // if (popupRect != null)
        // {
        //     // Anchor to top-right and set pivot to top-right; adjust offset as needed
        //     popupRect.anchorMin = popupRect.anchorMax = new Vector2(1f, 1f);
        //     popupRect.pivot = new Vector2(1f, 1f);
        //     popupRect.anchoredPosition = new Vector2(-10f, -10f);
        // }
        //
        // var popupComp = m_popup.GetComponent<Popup>();
        // if (popupComp != null) popupComp.Open();
    }
}