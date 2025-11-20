using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace QuestSystem
{
    public class GameProgressChecker : MonoBehaviour
    {
        public string customSlotDirectory;
        [SerializeField] private AssetReferenceSpreadsheet questSpreadsheet;
        private ES3Spreadsheet progressSheet;
        private int workIteration;
        private int homeIteration;
        private string currentLocation;
        private string lastLocation;
        private int currentQuest;
        private int currentQuestStage;

        private void Awake()
        {
            customSlotDirectory = CurrentSaveDirectory.CurrentDirectory;
            Debug.Log(customSlotDirectory);
            GetProgressSpreadsheet();
            GetDayIteration();
        }

        private void GetProgressSpreadsheet()
        {
            if (customSlotDirectory != null)
            {
                // Load the progress data into the ES3Spreadsheet
                var sheet = new ES3Spreadsheet();
                sheet.Load($"{customSlotDirectory}/myProgressData.csv");
                progressSheet = sheet;
            }
            else
            {
                Debug.Log("Custom Slot Directory is null.");
            }
        }

        private void GetDayIteration()
        {
            for (int i = 0; i < progressSheet.ColumnCount; i++)
            {

                string rawColumnName = progressSheet.GetCell<string>(i, 0);
                string columnName = rawColumnName?.Trim();

                switch (columnName)
                {
                    // Get workIteration
                    case "workIteration":
                        workIteration = progressSheet.GetCell<int>(i, 1);
                        Debug.Log("workIteration: " + workIteration);
                        break;

                    // Get homeIteration
                    case "homeIteration":
                        homeIteration = progressSheet.GetCell<int>(i, 1);
                        Debug.Log("workIteration: " + workIteration);
                        break;

                    // Set currentLocation to the current scene
                    case "currentLocation":
                        currentLocation = SceneManager.GetActiveScene().name;
                        progressSheet.SetCell(i, 1, currentLocation);
                        Debug.Log("currentLocation in spreadsheet is now " + currentLocation);
                        break;

                    // Set lastLocation to null
                    case "lastLocation":
                        progressSheet.SetCell(i, 1, "null");
                        Debug.Log("Reached Switch Case for lastLocation");
                        break;

                    // Get currentQuest
                    case "currentQuest":
                        currentQuest = progressSheet.GetCell<int>(i, 1);
                        Debug.Log("currentQuest: " + currentQuest);
                        break;

                    // Set currentQuestStage to 0
                    case "currentQuestStage":
                        currentQuestStage = progressSheet.GetCell<int>(i, 1);
                        Debug.Log("currentQuestStage: " + currentQuest);
                        break;
                }
            }
        }
    }
}