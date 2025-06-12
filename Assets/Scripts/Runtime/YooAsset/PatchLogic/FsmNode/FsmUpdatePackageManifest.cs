using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

public class FsmUpdatePackageManifest : IStateNode
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
        Debug.Log($"更新资源清单{data.packageName}");
        GameManager.Inst.StartCoroutine(UpdateManifest());
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    private IEnumerator UpdateManifest()
    {
        var packageName = data.packageName;
        var packageVersion = (string)_machine.GetBlackboardValue($"{packageName}_Version");
        var package = YooAssets.GetPackage(packageName);
        var operation = package.UpdatePackageManifestAsync(packageVersion);
        yield return operation;

        if (operation.Status != EOperationStatus.Succeed)
        {
            Debug.LogWarning(operation.Error);
            MessageBox.Show()
                    .SetTitle(packageName)
                    .SetContent(operation.Error)
                    .AddButton("退出", (box) => { Application.Quit(); });
            yield break;
        }
        else
        {
            Debug.Log($"{packageName}资源清单更新成功");
            _machine.ChangeState<FsmCreateDownloader>();
        }
    }
}