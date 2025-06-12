using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

internal class FsmRequestPackageVersion : IStateNode
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
        Debug.Log($"更新版本信息{data.packageName}");
        GameManager.Inst.StartCoroutine(UpdatePackageVersion());
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }

    private IEnumerator UpdatePackageVersion()
    {
        var packageName = data.packageName;
        var package = YooAssets.GetPackage(packageName);
        var operation = package.RequestPackageVersionAsync(true,10);
        var curVersion = PlayerPrefs.GetString($"{packageName}_Version", string.Empty);
        yield return operation;

        if (operation.Status != EOperationStatus.Succeed || data.useLocal)
        {
            Debug.LogWarning(operation.Error);
            if (string.IsNullOrEmpty(curVersion))
            {
                MessageBox.Show()
                    .SetTitle(packageName)
                    .SetContent(operation.Error)
                    .AddButton("退出", (box) => { Application.Quit(); });
            }
            else
            {
                Debug.Log($"{packageName}获取上次成功记录的版本{curVersion}");
                _machine.SetBlackboardValue($"{packageName}_Version", curVersion);
                _machine.ChangeState<FsmUpdatePackageManifest>();
            }
        }
        else
        {
            Debug.Log($"{packageName}获取远端资源版本成功{operation.PackageVersion}");
            _machine.SetBlackboardValue($"{packageName}_Version", operation.PackageVersion);
            _machine.ChangeState<FsmUpdatePackageManifest>();
        }
    }
}