using TMPro;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class SortManager : MonoBehaviour
{

    public int totalSorted = 0;
    int totalNeeded = 15;

    public TextMeshProUGUI quotaUI;
    public TextMeshProUGUI bayUI;

    private void Update()
    { 

        quotaUI.text = totalSorted.ToString() + "/" + totalNeeded.ToString();

    }

}
