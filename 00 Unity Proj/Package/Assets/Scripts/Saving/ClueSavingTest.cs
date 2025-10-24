using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ClueSavingTest : MonoBehaviour
{
    [SerializeField] private AssetReferenceSpreadsheet clueSpreadsheet;

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
            
            
            // // Save some values into the spreadsheet.
            // sheet.SetCell(0,0, "hitpoints");
            // sheet.SetCell(1,0, 12);
            // sheet.SetCell(0,1, "position");
            // sheet.SetCell(1,0, transform.position);
            //
            // // Save the file to a CSV.
            // sheet.Save("myClueData.csv");
            
            // sheet.Load(clueData.ToString());
            //sheet.Load(clueData);
            // Debug.Log("Clue Spreadsheet Loaded: " + clueData.name);
            // Debug.Log("Here is a debug of the sheet to a to a string: " + sheet.ToString());
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