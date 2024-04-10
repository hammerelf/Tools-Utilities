//Created by: Ryan King

using UnityEditor;
using UnityEngine;
using System.IO;
using HammerElf.Tools.Utilities;

public class AssetImportPostProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string assetPath in importedAssets)
        {
            ConsoleLog.Log("Checking path on: " + assetPath);
            if(assetPath.StartsWith("Packages/com.hammerelf.tools.utilities/UnityPackages") && assetPath.EndsWith(".unitypackage"))
            {
                AssetDatabase.ImportPackage(assetPath, false);
            }
        }
        //if (!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
        //{
        //    AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
        //}

        //// Loop through all imported assets
        //foreach (string assetPath in importedAssets)
        //{
        //    if (assetPath.EndsWith(".txt"))
        //    {
        //        string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(assetPath);

        //        FileUtil.CopyFileOrDirectory(assetPath, destinationPath);
        //        ConsoleLog.Log("Moved text file: " + assetPath + " to " + destinationPath);
        //    }
        //}

        ////Should be this scripts location
        //AssetDatabase.DeleteAsset("Packages/com.hammerelf.tools.utilities/Editor/AssetImportPostProcessor.cs");
        //AssetDatabase.Refresh();
    }
}