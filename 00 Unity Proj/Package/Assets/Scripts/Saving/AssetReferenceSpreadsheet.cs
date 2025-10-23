using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
[System.Serializable]
public class AssetReferenceSpreadsheet : AssetReferenceT<TextAsset>
{
    public AssetReferenceSpreadsheet(string guid) : base(guid)
    {
        
    }
}