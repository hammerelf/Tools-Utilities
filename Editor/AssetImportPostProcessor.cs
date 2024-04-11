//Created by: Ryan King

using UnityEditor;
using System.IO;
using System.Collections;
using Unity.EditorCoroutines.Editor;

[InitializeOnLoad]
public class AssetImportPostProcessor : AssetPostprocessor
{
    static bool isSubscribed = false;

    static AssetImportPostProcessor()
    {
        if (!isSubscribed)
        {
            EditorApplication.projectChanged += OnProjectChanged;
            isSubscribed = true;
        }
    }

    static void UnsubscribeFromProjectChanged()
    {
        if (isSubscribed)
        {
            EditorApplication.projectChanged -= OnProjectChanged;
            isSubscribed = false;
        }
    }

    static void OnProjectChanged()
    {
        string[] txtFiles = Directory.GetFiles("Packages/com.hammerelf.tools.utilities/Runtime/ScriptTemplates", "*.txt");

        if (!AssetDatabase.IsValidFolder("Assets/ScriptTemplates"))
        {
            AssetDatabase.CreateFolder("Assets", "ScriptTemplates");
        }

        EditorCoroutineUtility.StartCoroutineOwnerless(CopyAfterDelay(txtFiles, "Assets/ScriptTemplates"));
    }

    static IEnumerator CopyAfterDelay(string[] filePaths, string destinationPath)
    {
        while(AssetDatabase.IsValidFolder(destinationPath))
        {
            yield return null;
        }

        foreach (string txtFile in filePaths)
        {
            FileUtil.CopyFileOrDirectory(txtFile, destinationPath);
        }
        AssetDatabase.Refresh();
    }

    void OnDisable()
    {
        UnsubscribeFromProjectChanged();
    }

    void OnDestroy()
    {
        UnsubscribeFromProjectChanged();
    }
}