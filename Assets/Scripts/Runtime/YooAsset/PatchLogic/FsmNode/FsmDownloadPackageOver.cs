using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniFramework.Machine;
using YooAsset;

internal class FsmDownloadPackageOver : IStateNode
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
        Debug.Log($"{data.packageName}下载完成");
        // 注意：下载完成之后再保存本地版本
        if (data.playMode != EPlayMode.EditorSimulateMode)
        {
            var packageVersion = (string)_machine.GetBlackboardValue($"{data.packageName}_Version");
            PlayerPrefs.SetString($"{data.packageName}_Version", packageVersion);
            Debug.Log($"{data.packageName} 更新流程完毕 保存版本{packageVersion}");
        }
        _machine.ChangeState<FsmClearCacheBundle>();
    }
    void IStateNode.OnUpdate()
    {
    }
    void IStateNode.OnExit()
    {
    }
}