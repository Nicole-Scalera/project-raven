using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ClueSavingTest : MonoBehaviour
{
    [SerializeField] private AssetReferenceSpreadsheet clueSpreadsheet;

    private void Start()
    {
        clueSpreadsheet.LoadAssetAsync().Completed += OnClueSpreadsheetLoaded;
    }

    public void OnClueSpreadsheetLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            TextAsset clueData = handle.Result;
            var sheet = new ES3Spreadsheet();
            Debug.Log(clueData.ToString());
            // sheet.Load(clueData.ToString());
            // Debug.Log("Clue Spreadsheet Loaded: " + clueData.name);
            // Debug.Log("Here is a debug of the sheet to a to a string: " + sheet.ToString());
        }
        else
        {
            Debug.LogError("Failed to load clue spreadsheet.");
        }
    }
}