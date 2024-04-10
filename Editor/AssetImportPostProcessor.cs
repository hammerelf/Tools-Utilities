//Created by: Ryan King

using UnityEditor;
using UnityEngine;
using System.IO;
using HammerElf.Tools.Utilities;

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
        foreach (string txtFile in txtFiles)
        {
            FileUtil.CopyFileOrDirectory(txtFile, "Assets/ScriptTemplates");
        }
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