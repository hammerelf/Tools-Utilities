//Created by: Ryan King

using UnityEditor;
using UnityEngine;
using System.IO;
using HammerElf.Tools.Utilities;

//public class AssetImportPostProcessor : AssetPostprocessor
//{
//	//Will create a folder under Assets called ScriptTemplates. All files under the ScriptTemplates
//	//folder in the package will be moved to that folder.
//	private void OnPreprocessAsset()
//	{
//		ConsoleLog.Log("Start preprocess on path : " + assetImporter.assetPath);
//        //AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
//        //FileUtil.CopyFileOrDirectory("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "Assets/ScriptTemplates");

//        //AssetDatabase.ImportAsset("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates/80-C#_Clean-NewCleanBehaviourScript.cs.txt");

//        if (assetImporter.assetPath.StartsWith("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates"))
//        {
//			ConsoleLog.Log("Matched asset to process: \n" + assetImporter.assetPath);
//            if (!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
//            {
//				AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
//            }

//            // Check if the imported asset is one of the text files you want to move
//            if (assetImporter.assetPath.EndsWith(".txt") || assetImporter.assetPath.EndsWith(".meta")) // You can adjust the condition as needed
//			{
//				// Define the destination path in the Assets folder
//				string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(assetImporter.assetPath);

//				if (!File.Exists(destinationPath))
//				{
//					FileUtil.CopyFileOrDirectory(assetImporter.assetPath, destinationPath);
//					AssetDatabase.Refresh();

//					ConsoleLog.Log("Moved text file: " + assetImporter.assetPath + " to " + destinationPath);
//				}
//				else
//				{
//					ConsoleLog.Log("File already exists at: " + destinationPath);
//				}
//			}
//		}
//	}

//	//private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//	//{
//	//	//AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
//	//	//FileUtil.CopyFileOrDirectory("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "Assets/ScriptTemplates");

//	//	//AssetDatabase.ImportAsset("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates/80-C#_Clean-NewCleanBehaviourScript.cs.txt");


//	//	// Loop through all imported assets
//	//	foreach (string assetPath in importedAssets)
//	//	{
//	//		// Check if the imported asset is one of the text files you want to move
//	//		if (assetPath.EndsWith(".txt") || assetPath.EndsWith(".meta")) // You can adjust the condition as needed
//	//		{
//	//			// Define the destination path in the Assets folder
//	//			string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(assetPath);

//	//			if(!File.Exists(destinationPath))
//	//			{
//	//				FileUtil.CopyFileOrDirectory(assetPath, destinationPath);
//	//				AssetDatabase.Refresh();

//	//				ConsoleLog.Log("Moved text file: " + assetPath + " to " + destinationPath);
//	//			}
//	//			else
//	//			{
//	//				ConsoleLog.Log("File already exists at: " + destinationPath);
//	//			}
//	//		}
//	//	}
//	//}
//}


//public class AssetImportPostProcessor : AssetModificationProcessor
//{
//    // Called before importing assets. This method allows you to modify the list of assets to be imported.
//    public static string[] OnWillSaveAssets(string[] paths)
//    {
//        foreach (string path in paths)
//        {
//            if (path.EndsWith(".txt"))
//            {
//                ConsoleLog.Log("Text file imported: " + path);
//                MoveTextFileToScriptTemplates(path);
//            }
//        }
//        return paths;
//    }

//    // Move the text file to the ScriptTemplates folder
//    private static void MoveTextFileToScriptTemplates(string sourcePath)
//    {
//		//if(!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
//		//{
//		//	AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
//		//}

//        string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(sourcePath);
//        if (!File.Exists(destinationPath))
//        {
//            FileUtil.CopyFileOrDirectory(sourcePath, destinationPath);
//            AssetDatabase.Refresh();
//            ConsoleLog.Log("Moved text file: " + sourcePath + " to " + destinationPath);
//        }
//        else
//        {
//            ConsoleLog.Log("File already exists at: " + destinationPath);
//        }
//    }
//}



//[InitializeOnLoad]
//public class AssetImportPostProcessor : AssetPostprocessor
//{
//    static bool isSubscribed = false;

//    static AssetImportPostProcessor()
//    {
//        // Subscribe to project change events to detect when packages are imported
//        SubscribeToProjectChanged();
//    }

//    static void SubscribeToProjectChanged()
//    {
//        if (!isSubscribed)
//        {
//            EditorApplication.projectChanged += OnProjectChanged;
//            isSubscribed = true;
//        }
//    }

//    static void UnsubscribeFromProjectChanged()
//    {
//        if (isSubscribed)
//        {
//            EditorApplication.projectChanged -= OnProjectChanged;
//            isSubscribed = false;
//        }
//    }

//    static void OnProjectChanged()
//    {
//        string[] txtFiles = Directory.GetFiles("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "*.txt");
//        foreach (string txtFile in txtFiles)
//        {
//            // Import each .txt file explicitly to trigger OnPreprocessAsset
//            AssetDatabase.ImportAsset("Assets" + txtFile.Substring(Application.dataPath.Length));
//        }


//        //if(!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
//        //{
//        //	AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
//        //}

//        string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(sourcePath);
//        if (!File.Exists(destinationPath))
//        {
//            FileUtil.CopyFileOrDirectory(sourcePath, destinationPath);
//            AssetDatabase.Refresh();
//            ConsoleLog.Log("Moved text file: " + sourcePath + " to " + destinationPath);
//        }
//        else
//        {
//            ConsoleLog.Log("File already exists at: " + destinationPath);
//        }
//    }

//    void OnDisable()
//    {
//        UnsubscribeFromProjectChanged();
//    }

//    void OnDestroy()
//    {
//        UnsubscribeFromProjectChanged();
//    }
//}


public class AssetImportPostProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        //AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
        //FileUtil.CopyFileOrDirectory("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "Assets/ScriptTemplates");

        //AssetDatabase.ImportAsset("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates/80-C#_Clean-NewCleanBehaviourScript.cs.txt");
        
        if (!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
        {
            AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
        }

        // Loop through all imported assets
        foreach (string assetPath in importedAssets)
        {
            // Check if the imported asset is one of the text files you want to move
            if (assetPath.EndsWith(".txt")) // You can adjust the condition as needed
            {
                // Define the destination path in the Assets folder
                string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(assetPath);

                //if (!File.Exists(destinationPath))
                //{
                    FileUtil.CopyFileOrDirectory(assetPath, destinationPath);

                    ConsoleLog.Log("Moved text file: " + assetPath + " to " + destinationPath);
                //}
                //else
                //{
                //    ConsoleLog.Log("File already exists at: " + destinationPath);
                //}
            }
        }

        AssetDatabase.DeleteAsset("Packages/com.hammerelf.tools.utilities/Editor/AssetImportPostProcessor.cs");
        AssetDatabase.Refresh();
    }
}