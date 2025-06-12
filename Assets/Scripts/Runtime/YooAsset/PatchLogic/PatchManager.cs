using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class PatchManager : Singleton<PatchManager>
{
    public PatchOperationData PreloadData(EPlayMode PlayMode)
    {
        PatchOperationData preload = new PatchOperationData();
        preload.packageName = "Preload";
        preload.playMode = PlayMode;
        preload.autoDownload = true;
        //preload.downloadError = DefDownloadError;
        //preload.downloadFinish = DefDownloadFinsh;
        return preload;
    }

    public PatchOperationData MainData(EPlayMode PlayMode, DownloaderOperation.DownloadUpdate DownloadUpdate)
    {
        PatchOperationData main = new PatchOperationData();
        main.packageName = "Main";
        main.playMode = PlayMode;
        main.autoDownload = false;
        //main.downloadError = DefDownloadError;
        main.downloadUpdate = DownloadUpdate;
        //main.downloadFinish = DefDownloadFinsh;
        return main;
    }

    //默认下载错误回调
    //public void DefDownloadError(DownloadErrorData downloadErrorData)
    //{
    //    MessageBox.Show()
    //        .SetTitle(packageName)
    //        .SetContent(downloadErrorData.ErrorInfo)
    //        .AddButton("退出", (box) => { Application.Quit(); });
    //}
}
