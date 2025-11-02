using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// This class acts as a save manager to load and save important
// data that should persist between play sessions.

public class SaveManager : MonoBehaviour
{
    [SerializeField] private AssetReferenceSpreadsheet clueSpreadsheet;
    [SerializeField] private AssetReferenceSpreadsheet progressSpreadsheet;

    private void Start()
    {
        // Load the spreadsheets asynchronously and register a callback upon completion
        clueSpreadsheet.LoadAssetAsync().Completed += OnClueSpreadsheetLoaded;
        // LoadClueSpreadsheet();
        progressSpreadsheet.LoadAssetAsync().Completed += OnProgressSpreadsheetLoaded;
        // LoadProgressSpreadsheet();
    }

    // When clueSpreadsheet is loaded, load the data into an ES3Spreadsheet
    public void OnClueSpreadsheetLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TextAsset clueData = handle.Result;
            var sheet = new ES3Spreadsheet();
            
            // Load from TextAsset text (uses default ES3Settings/encoding)
            sheet.LoadRaw(clueData.text);

            // Save the loaded spreadsheet to disk
            sheet.Save($"slots/myClueData.csv");
        }
        else
        {
            Debug.LogError("Failed to load clue spreadsheet.");
        }
    }
    
    // When progressSpreadsheet is loaded, load the data into an ES3Spreadsheet
    public void OnProgressSpreadsheetLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TextAsset progressData = handle.Result;
            var sheet = new ES3Spreadsheet();
            
            // Load from TextAsset text (uses default ES3Settings/encoding)
            sheet.LoadRaw(progressData.text);

            // Save the loaded spreadsheet to disk
            sheet.Save("slots/myProgressData.csv");
        }
        else
        {
            Debug.LogError("Failed to load progress data spreadsheet.");
        }
    }
    
    // // This will load a previously saved ES3Spreadsheet from disk
    // public void LoadClueSpreadsheet()
    // {
    //     var sheet = new ES3Spreadsheet();
    //     sheet.Load("myClueData.csv");
    // }
    //
    // public void LoadProgressSpreadsheet()
    // {
    //     var sheet = new ES3Spreadsheet();
    //     sheet.Load("myProgressData.csv");
    // }
}