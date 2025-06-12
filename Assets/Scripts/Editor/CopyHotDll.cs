using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Text;


public class CopyHotDll
{
    [MenuItem("Tools/更新生成PreloadDll")]
    public static void CopyPreloadDll2Byte()
    {
        HybridCLR.Editor.Commands.CompileDllCommand.CompileDllActiveBuildTarget();
        string sourceDir = $"{Application.dataPath.Replace("/Assets", "")}/HybridCLRData/HotUpdateDlls/{UnityEditor.EditorUserBuildSettings.activeBuildTarget}/Preload.dll";
        string destDir = $"{Application.dataPath}/Res/Preload/HotUpdateDll/Preload.bytes";
        if (File.Exists(destDir))
        {
            File.Delete(destDir);
        }
        File.Copy(sourceDir, destDir);
        AssetDatabase.Refresh();
        Debug.Log($"copy {sourceDir} to {destDir}");
    }
    [MenuItem("Tools/更新生成MainDll")]
    public static void CopyMainDll2Byte()
    {
        HybridCLR.Editor.Commands.CompileDllCommand.CompileDllActiveBuildTarget();
        string sourceDir = $"{Application.dataPath.Replace("/Assets", "")}/HybridCLRData/HotUpdateDlls/{UnityEditor.EditorUserBuildSettings.activeBuildTarget}/Main.dll";
        string destDir = $"{Application.dataPath}/Res/Main/HotUpdateDll/Main.bytes";
        if (File.Exists(destDir))
        {
            File.Delete(destDir);
        }
        File.Copy(sourceDir, destDir);
        AssetDatabase.Refresh();
        Debug.Log($"copy {sourceDir} to {destDir}");
    }
    [MenuItem("Tools/更新生成补充数据源")]
    public static void CopyDepDll2Byte()
    {
        HybridCLR.Editor.Commands.CompileDllCommand.CompileDllActiveBuildTarget();
        string sourceDir = $"{Application.dataPath.Replace("/Assets", "")}/HybridCLRData/AssembliesPostIl2CppStrip/{UnityEditor.EditorUserBuildSettings.activeBuildTarget}/";
        string destDir = $"{Application.dataPath}/Res/Main/HotUpdateDll/";
        foreach (string dll in Boot.Inst.DepDlls)
        {
            string sourcePath = $"{sourceDir}/{dll}";
            string destPath = $"{destDir}/{dll}.bytes";
            if (File.Exists(sourcePath))
            {
                if (File.Exists(destPath))
                {
                    File.Delete(destPath);
                }
                File.Copy(sourcePath, destPath);
                AssetDatabase.Refresh();
                Debug.Log($"copy {sourcePath} to {destPath}");
            }
        }
        Debug.Log("copy over");
    }
}
