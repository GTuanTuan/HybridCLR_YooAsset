using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Event;
using YooAsset;

public class Boot : Singleton<Boot>
{
    public GameObject MainUICanvas;
    public Camera UICamera;
    public List<string> DepDlls = new List<string>()
    {
        "mscorlib.dll",
        "System.dll",
        "System.Core.dll",
        "Mirror.dll"
    };
    public List<GameObject> DontDestroyObjs = new List<GameObject>();
    /// <summary>
    /// 资源系统运行模式
    /// </summary>
    public EPlayMode PlayMode = EPlayMode.EditorSimulateMode;

    void Awake()
    {
        Debug.Log($"资源系统运行模式：{PlayMode}");
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
    }
    IEnumerator Start()
    {
        foreach (GameObject obj in DontDestroyObjs)
        {
            DontDestroyOnLoad(obj);
        }
        GameManager.Inst.Behaviour = this;
        GameManager.Inst.MainUICanvas = MainUICanvas;
        GameManager.Inst.UICamera = UICamera;
        GameManager.Inst.LoadServerSettings();

        YooAssets.Initialize();

        //更新Preload
        var operation = new PatchOperation(PatchManager.Inst.PreloadData(PlayMode));
        YooAssets.StartOperation(operation);
        yield return operation;

        //加载更新界面
        GameManager.Inst.LoadDll(YooAssets.GetPackage("Preload"), "Preload");
        AssetHandle patchWindowHandle = YooAssets.GetPackage("Preload").LoadAssetAsync<GameObject>("PatchWindow");
        patchWindowHandle.InstantiateAsync(GameManager.Inst.MainUICanvas.transform);
    }
}