using System.Collections;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

public class FsmDownloadPackageFiles : IStateNode
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
        Debug.Log($"{data.packageName}开始下载资源文件");
        GameManager.Inst.StartCoroutine(BeginDownload());
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    private IEnumerator BeginDownload()
    {
        var downloader = (ResourceDownloaderOperation)_machine.GetBlackboardValue($"{data.packageName}_Downloader");
        downloader.BeginDownload();
        yield return downloader;

        // 检测下载结果
        if (downloader.Status != EOperationStatus.Succeed)
        {
            MessageBox.Show()
                    .SetTitle(data.packageName)
                    .SetContent($"下载失败{downloader.Error}")
                    .AddButton("退出", (box) => { Application.Quit(); });
            yield break;
        }
        _machine.ChangeState<FsmDownloadPackageOver>();
    }
}