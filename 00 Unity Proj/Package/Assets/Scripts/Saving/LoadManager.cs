using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadManager : MonoBehaviour
{
    // This will load a previously saved ES3Spreadsheet from disk
    public void LoadClueSpreadsheet()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("myClueData.csv");
    }
    
    public void LoadProgressSpreadsheet()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("myProgressData.csv");
    }
}