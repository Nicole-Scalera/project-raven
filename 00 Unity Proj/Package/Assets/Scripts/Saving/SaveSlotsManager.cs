using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// This is the save slot manager. It inherits from ES3SlotManager.

public class SaveSlotsManager : ES3SlotManager
{
    public string customSlotDirectory = "";
    [SerializeField] private AssetReferenceSpreadsheet clueSpreadsheet;
    [SerializeField] private AssetReferenceSpreadsheet progressSpreadsheet;

    private void DownloadStarterData()
    {
        // Load the spreadsheets asynchronously and register a callback upon completion
        clueSpreadsheet.LoadAssetAsync().Completed += OnClueSpreadsheetLoaded;
        progressSpreadsheet.LoadAssetAsync().Completed += OnProgressSpreadsheetLoaded;
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
            sheet.Save($"{customSlotDirectory}/myClueData.csv");
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
            sheet.Save($"{customSlotDirectory}/myProgressData.csv");
        }
        else
        {
            Debug.LogError("Failed to load progress data spreadsheet.");
        }
    }
    
    public override ES3Slot CreateNewSlot(string slotName)
    {
        // Get the current timestamp.
        var creationTimestamp = DateTime.Now;
        
        // Create the slot in the UI.
        var slot = InstantiateSlot(slotName, creationTimestamp);
        
        // Move the slot to the top of the list.
        slot.MoveToTop();
        
        ES3.SaveRaw("{123}", GetSlotPath(slotName));

        Debug.Log("SaveSlotsManager > CreateNewSlot: Created new slot with name: " + slotName);
        
        // Create a sub-directory for this save based on its name
        // and remove any whitespaces from the path
        customSlotDirectory = RemoveWhitespaceSlotDirectory(slotName);
        Debug.Log("SaveSlotsManager > RemoveWhitespaceSlotDirectory > customSlotDirectory is " + customSlotDirectory);

        DownloadStarterData();
        
        // Select the slot if necessary.
        if (selectSlotAfterCreation)
            slot.SelectSlot();

        // Scroll the scroll view to the top of the list.
        ScrollToTop();

        return slot;
    }
    
    // Take any whitespace out of the slot directory name
    public string RemoveWhitespaceSlotDirectory(string slotName)
    {
        // We convert any whitespace characters to underscores at this point to make the file more portable.
        return Regex.Replace(slotName, @"\s+", "_");
    }
    
}