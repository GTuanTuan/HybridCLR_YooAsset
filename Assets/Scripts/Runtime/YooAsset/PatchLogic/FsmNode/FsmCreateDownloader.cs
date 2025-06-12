using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

public class FsmCreateDownloader : IStateNode
{
    private StateMachine _machine;
    PatchOperationData data;
    void IStateNode.OnCreate(StateMachine machine)
    {
        _machine = machine;
    }
    void IStateNode.OnEnter()
    {
        data = (PatchOperationData)_machine.GetBlackboardValue("PatchOperationData");
        Debug.Log($"创建资源下载器{data.packageName}");
        CreateDownloader();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    void CreateDownloader()
    {
        var packageName = data.packageName;
        var package = YooAssets.GetPackage(packageName);
        var downloader = package.CreateResourceDownloader(10, 3);
        downloader.DownloadErrorCallback = data.downloadError;
        downloader.DownloadFinishCallback = data.downloadFinish;
        downloader.DownloadUpdateCallback = data.downloadUpdate;
        var curVersion = PlayerPrefs.GetString($"{packageName}_Version", string.Empty);
        var packageVersion = (string)_machine.GetBlackboardValue($"{packageName}_Version");
        _machine.SetBlackboardValue($"{packageName}_Downloader", downloader);

        if (downloader.TotalDownloadCount == 0)
        {
            Debug.Log("Not found any download files !");
            _machine.ChangeState<FsmStartGame>();
        }
        else
        {
            if (data.autoDownload)
            {
                _machine.ChangeState<FsmDownloadPackageFiles>();
            }
            else 
            {
                MessageBox.Show()
                 .SetTitle(packageName)
                 .SetContent($"发现资源更新\n{curVersion}=>{packageVersion}: {downloader.TotalDownloadBytes / 1024f / 1024f:F1}MB")
                 .AddButton("下载", (box) => { _machine.ChangeState<FsmDownloadPackageFiles>(); })
                 .AddButton("取消", (box) => 
                 { 
                     downloader.CancelDownload();
                     data.useLocal = true;
                     _machine.SetBlackboardValue("PatchOperationData", data);
                     _machine.ChangeState<FsmRequestPackageVersion>(); 
                 })
                 .AddButton("退出", (box) => { Application.Quit(); });
            }
        }
    }
}