using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// This class allows you to create an Addressable Asset Reference
// for TextAsset spreadsheets. It was created since ES3Spreadsheet
// works with TextAssets for loading and saving data.

[System.Serializable]
public class AssetReferenceSpreadsheet : AssetReferenceT<TextAsset>
{
    public AssetReferenceSpreadsheet(string guid) : base(guid)
    {
        
    }
}