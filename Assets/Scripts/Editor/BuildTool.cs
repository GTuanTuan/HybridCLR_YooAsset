using UnityEditor;
using UnityEngine;
using YooAsset.Editor;
using YooAsset;
using System;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;

public class BuildTool
{
    [MenuItem("Tools/���Preload")]
    public static void BuildPreload()
    {
        CopyHotDll.CopyPreloadDll2Byte();
        ExecuteBuild("Preload",EBuildPipeline.ScriptableBuildPipeline, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log($"���Preload����");
    }
    [MenuItem("Tools/���Main %G")]
    public static void BuildMain()
    {
        CopyHotDll.CopyMainDll2Byte();
        ExecuteBuild("Main", EBuildPipeline.ScriptableBuildPipeline, EditorUserBuildSettings.activeBuildTarget);
        Debug.Log($"���Main����");
    }
    [MenuItem("Tools/ȫ�����")]
    public static void BuildAll()
    {
        BuildPreload();
        BuildMain();
    }

    public static void ExecuteBuild(string PackageName, EBuildPipeline BuildPipeline, BuildTarget BuildTarget)
    {
        var fileNameStyle = AssetBundleBuilderSetting.GetPackageFileNameStyle(PackageName, BuildPipeline);
        var buildinFileCopyOption = AssetBundleBuilderSetting.GetPackageBuildinFileCopyOption(PackageName, BuildPipeline);
        var buildinFileCopyParams = AssetBundleBuilderSetting.GetPackageBuildinFileCopyParams(PackageName, BuildPipeline);
        var compressOption = AssetBundleBuilderSetting.GetPackageCompressOption(PackageName, BuildPipeline);
        var clearBuildCache = AssetBundleBuilderSetting.GetPackageClearBuildCache(PackageName, BuildPipeline);
        var useAssetDependencyDB = AssetBundleBuilderSetting.GetPackageUseAssetDependencyDB(PackageName, BuildPipeline);
        var builtinShaderBundleName = GetBuiltinShaderBundleName(PackageName);

        ScriptableBuildParameters buildParameters = new ScriptableBuildParameters();
        buildParameters.BuildOutputRoot = AssetBundleBuilderHelper.GetDefaultBuildOutputRoot();
        buildParameters.BuildinFileRoot = AssetBundleBuilderHelper.GetStreamingAssetsRoot();
        buildParameters.BuildPipeline = BuildPipeline.ToString();
        buildParameters.BuildBundleType = (int)EBuildBundleType.AssetBundle;
        buildParameters.BuildTarget = BuildTarget;
        buildParameters.PackageName = PackageName;
        buildParameters.PackageVersion = GetPackageVersion();
        buildParameters.EnableSharePackRule = true;
        buildParameters.VerifyBuildingResult = true;
        buildParameters.FileNameStyle = fileNameStyle;
        buildParameters.BuildinFileCopyOption = buildinFileCopyOption;
        buildParameters.BuildinFileCopyParams = buildinFileCopyParams;
        buildParameters.CompressOption = compressOption;
        buildParameters.ClearBuildCacheFiles = clearBuildCache;
        buildParameters.UseAssetDependencyDB = useAssetDependencyDB;
        buildParameters.BuiltinShadersBundleName = builtinShaderBundleName;
        buildParameters.EncryptionServices = CreateEncryptionInstance(PackageName, BuildPipeline);

        ScriptableBuildPipeline pipeline = new ScriptableBuildPipeline();
        var buildResult = pipeline.Run(buildParameters, true);
        if (buildResult.Success)
            EditorUtility.RevealInFinder(buildResult.OutputPackageDirectory);
    }

    public static string GetPackageVersion()
    {
        int totalMinutes = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
        return DateTime.Now.ToString("yyyy-MM-dd") + "-" + totalMinutes;
    }
    /// <summary>
    /// ������ɫ����Դ������
    /// ע�⣺���Զ��ռ�����ɫ����Դ��������һ�£�
    /// </summary>
    public static string GetBuiltinShaderBundleName(string PackageName)
    {
        var uniqueBundleName = AssetBundleCollectorSettingData.Setting.UniqueBundleName;
        var packRuleResult = DefaultPackRule.CreateShadersPackRuleResult();
        return packRuleResult.GetBundleName(PackageName, uniqueBundleName);
    }

    public static IEncryptionServices CreateEncryptionInstance(string PackageName, EBuildPipeline BuildPipeline)
    {
        var encyptionClassName = AssetBundleBuilderSetting.GetPackageEncyptionClassName(PackageName, BuildPipeline);
        var encryptionClassTypes = EditorTools.GetAssignableTypes(typeof(IEncryptionServices));
        var classType = encryptionClassTypes.Find(x => x.FullName.Equals(encyptionClassName));
        if (classType != null)
            return (IEncryptionServices)Activator.CreateInstance(classType);
        else
            return null;
    }
}


