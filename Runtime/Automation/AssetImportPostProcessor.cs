//Created by: Ryan King

using UnityEditor;
using UnityEngine;
using System.IO;
using HammerElf.Tools.Utilities;

public class AssetImportPostProcessor : AssetPostprocessor
{
	//Will create a folder under Assets called ScriptTemplates. All files under the ScriptTemplates
	//folder in the package will be moved to that folder.
	private void OnPreprocessAsset()
	{
		//AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
		//FileUtil.CopyFileOrDirectory("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "Assets/ScriptTemplates");

		//AssetDatabase.ImportAsset("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates/80-C#_Clean-NewCleanBehaviourScript.cs.txt");

		if (assetImporter.assetPath.StartsWith("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates"))
        {
			ConsoleLog.Log("Matched asset to process: \n" + assetImporter.assetPath);
            if (!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
            {
				AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
            }

            // Check if the imported asset is one of the text files you want to move
            if (assetImporter.assetPath.EndsWith(".txt") || assetImporter.assetPath.EndsWith(".meta")) // You can adjust the condition as needed
			{
				// Define the destination path in the Assets folder
				string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(assetImporter.assetPath);

				if (!File.Exists(destinationPath))
				{
					FileUtil.CopyFileOrDirectory(assetImporter.assetPath, destinationPath);
					AssetDatabase.Refresh();

					ConsoleLog.Log("Moved text file: " + assetImporter.assetPath + " to " + destinationPath);
				}
				else
				{
					ConsoleLog.Log("File already exists at: " + destinationPath);
				}
			}
		}
	}
		
	//private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	//{
	//	//AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
	//	//FileUtil.CopyFileOrDirectory("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "Assets/ScriptTemplates");
			
	//	//AssetDatabase.ImportAsset("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates/80-C#_Clean-NewCleanBehaviourScript.cs.txt");
			
			
	//	// Loop through all imported assets
	//	foreach (string assetPath in importedAssets)
	//	{
	//		// Check if the imported asset is one of the text files you want to move
	//		if (assetPath.EndsWith(".txt") || assetPath.EndsWith(".meta")) // You can adjust the condition as needed
	//		{
	//			// Define the destination path in the Assets folder
	//			string destinationPath = "Assets/ScriptTemplates/" + Path.GetFileName(assetPath);
					
	//			if(!File.Exists(destinationPath))
	//			{
	//				FileUtil.CopyFileOrDirectory(assetPath, destinationPath);
	//				AssetDatabase.Refresh();
						
	//				ConsoleLog.Log("Moved text file: " + assetPath + " to " + destinationPath);
	//			}
	//			else
	//			{
	//				ConsoleLog.Log("File already exists at: " + destinationPath);
	//			}
	//		}
	//	}
	//}
}
