using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using YooAsset;

public class PatchWindow : MonoBehaviour
{
    public Text statusText;
    public Slider progressBar;
    public Text downloadSizeText;
    public VideoPlayer video;
    private void Awake()
    {
        video.targetCamera = GameManager.Inst.UICamera;
    }
    IEnumerator Start()
    {
        GameManager.Inst.MainUICanvas.transform.Find("InitBg").gameObject.SetActive(false);
        //更新Main
        var operation = new PatchOperation(PatchManager.Inst.MainData(Boot.Inst.PlayMode, (data) =>
        {
            progressBar.value = data.Progress;
            downloadSizeText.text = $"{data.CurrentDownloadBytes / 1024f / 1024f:F1}MB/{data.TotalDownloadBytes / 1024f / 1024f:F1}MB";
        }));
        YooAssets.StartOperation(operation);
        yield return operation;

        YooAssets.SetDefaultPackage(YooAssets.GetPackage("Main"));
        GameManager.Inst.LoadDll(YooAssets.GetPackage("Main"), "Main");
        AssetHandle gameStartHandle = YooAssets.GetPackage("Main").LoadAssetAsync<GameObject>("GameStart");
        gameStartHandle.InstantiateAsync();
        gameObject.SetActive(false);
    }
}