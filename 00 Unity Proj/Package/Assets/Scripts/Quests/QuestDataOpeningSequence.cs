using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace QuestSystem
{
    public class QuestDataOpeningSequence : MonoBehaviour
    {
        public string customSlotDirectory;
        [SerializeField] private AssetReferenceSpreadsheet questSpreadsheet;
        // private ES3Spreadsheet progressSheet;

        private void Awake()
        {
            customSlotDirectory = CurrentSaveDirectory.CurrentDirectory;
            Debug.Log(customSlotDirectory);
            SetStarterQuestData();
        }

        private void SetStarterQuestData()
        {
            // Load the spreadsheets asynchronously and register a callback upon completion
            questSpreadsheet.LoadAssetAsync().Completed += OnQuestDataLoaded;
        }

        // When questSpreadsheet is loaded, load the data into an ES3Spreadsheet
        public void OnQuestDataLoaded(AsyncOperationHandle<TextAsset> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                TextAsset questData = handle.Result;
                var questSheet = new ES3Spreadsheet();
                
                // Load from TextAsset text (uses default ES3Settings/encoding)
                questSheet.LoadRaw(questData.text);
        
                // Save the loaded spreadsheet to disk
                questSheet.Save($"{customSlotDirectory}/myQuestData.csv");

                UpdateProgressQuestData(); // Now, update the progress quest data

            }
            else
            {
                Debug.LogError("Failed to load clue spreadsheet.");
            }
        }
        
        // Used to update the basic quest info in the progress data spreadsheet
        public void UpdateProgressQuestData()
        {
            // Load the progress data into the ES3Spreadsheet
            var progressSheet = new ES3Spreadsheet();
            progressSheet.Load($"{customSlotDirectory}/myProgressData.csv");
            
            string cellExample = progressSheet.GetCell<string>(0, 0); // Example of accessing a cell
            
            Debug.Log("0,0: " + cellExample);
            Debug.Log(progressSheet.ColumnCount);
            
            progressSheet.SetCell(4, 4, "RandomValue");

            int currentRow; // Current row
            int currentColumn; // Current column

            for (int i = 0; i < progressSheet.ColumnCount; i++)
            {
                string rawColumnName = progressSheet.GetCell<string>(i, 0);
                string columnName = rawColumnName?.Trim();
                
                // Debugging
                // Debug.Log(columnName);

                switch (columnName)
                {
                    // Set workIteration to 0
                    case "workIteration":
                        progressSheet.SetCell(i, 1, 0);
                        Debug.Log("Reached Switch Case for workIteration");
                        break;
                        
                    // Set homeIteration to 1
                    case "homeIteration":
                        progressSheet.SetCell(i, 1, 1);
                        Debug.Log("Reached Switch Case for homeIteration");
                        break;
                        
                    // Set currentLocation to null
                    case "currentLocation":
                        progressSheet.SetCell(i, 1, "null");
                        Debug.Log("Reached Switch Case for currentLocation");
                        break;
                        
                    // Set lastLocation to null
                    case "lastLocation":
                        progressSheet.SetCell(i, 1, "null");
                        Debug.Log("Reached Switch Case for lastLocation");
                        break;
                    
                    // Set currentQuest to 0
                    case "currentQuest":
                        progressSheet.SetCell(i, 1, 0);
                        Debug.Log("Reached Switch Case for currentQuest");
                        break;
                        
                    // Set currentQuestStage to 0
                    case "currentQuestStage":
                        progressSheet.SetCell(i, 1, 0);
                        Debug.Log("Reached Switch Case for currentQuestStage");
                        break;
                }
                
                progressSheet.Save($"{customSlotDirectory}/myProgressData.csv");
            }
        }
    }
}