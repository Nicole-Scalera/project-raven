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
        // Load the clueSpreadsheet asynchronously
        // and register a callback for when it's loaded
        clueSpreadsheet.LoadAssetAsync().Completed += OnClueSpreadsheetLoaded;
        LoadSavedSpreadsheet();
    }

    // When clueSpreadsheet is loaded, load the data into an ES3Spreadsheet
    public void OnClueSpreadsheetLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TextAsset clueData = handle.Result;
            // string assetPath = handle.Location?.InternalId ?? "unknown";
            var sheet = new ES3Spreadsheet();
            
            // Load from TextAsset text (uses default ES3Settings/encoding)
            sheet.LoadRaw(clueData.text);
            
            Debug.Log(sheet.ToString());
            Debug.Log("This is the row count: " + sheet.RowCount);
            Debug.Log("This is the column count: " + sheet.ColumnCount);
            
            Debug.Log("Here is 1,1: " + sheet.GetCell<string>(1, 1));
            
            sheet.Save("myClueData.csv");
        }
        else
        {
            Debug.LogError("Failed to load clue spreadsheet.");
        }
    }
    
    public void LoadSavedSpreadsheet()
    {
        var sheet = new ES3Spreadsheet();
        sheet.Load("myClueData.csv");
        Debug.Log("Loaded saved spreadsheet:");
        Debug.Log(sheet.ToString());
        Debug.Log("LoadSavedSpreadsheet > This is the row count: " + sheet.RowCount);
        Debug.Log("LoadSavedSpreadsheet > This is the column count: " + sheet.ColumnCount);
    }
}