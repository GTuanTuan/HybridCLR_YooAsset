using UnityEngine;
using UniFramework.Machine;
using UniFramework.Event;
using YooAsset;

public class PatchOperation : GameAsyncOperation
{
    private enum ESteps
    {
        None,
        Update,
        Done,
    }

    private readonly EventGroup _eventGroup = new EventGroup();
    private readonly StateMachine _machine;
    private readonly string _packageName;
    private ESteps _steps = ESteps.None;

    public PatchOperation(PatchOperationData data)
    {
        _packageName = data.packageName;
        // 创建状态机
        _machine = new StateMachine(this);
        _machine.AddNode<FsmInitializePackage>();
        _machine.AddNode<FsmRequestPackageVersion>();
        _machine.AddNode<FsmUpdatePackageManifest>();
        _machine.AddNode<FsmCreateDownloader>();
        _machine.AddNode<FsmDownloadPackageFiles>();
        _machine.AddNode<FsmDownloadPackageOver>();
        _machine.AddNode<FsmClearCacheBundle>();
        _machine.AddNode<FsmStartGame>();

        _machine.SetBlackboardValue("PatchOperationData", data);
    }
    protected override void OnStart()
    {
        _steps = ESteps.Update;
        _machine.Run<FsmInitializePackage>();
    }
    protected override void OnUpdate()
    {
        if (_steps == ESteps.None || _steps == ESteps.Done)
            return;

        if (_steps == ESteps.Update)
        {
            _machine.Update();
        }
    }
    protected override void OnAbort()
    {
    }

    public void SetFinish()
    {
        _steps = ESteps.Done;
        _eventGroup.RemoveAllListener();
        Status = EOperationStatus.Succeed;
        Debug.Log($"Package {_packageName} patch done !");
    }
}
public class PatchOperationData
{
    public string packageName;
    public EPlayMode playMode;
    public bool autoDownload;
    public bool useLocal;
    public DownloaderOperation.DownloadError downloadError;
    public DownloaderOperation.DownloaderFinish downloadFinish;
    public DownloaderOperation.DownloadUpdate downloadUpdate;
}