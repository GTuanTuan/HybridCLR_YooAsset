using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public class GameStart : MonoBehaviour
{
    SceneHandle sceneHandle;
    void Start()
    {
        StartCoroutine(LoadScene());
        StartCoroutine(LoadUIManager());
    }
    IEnumerator LoadScene()
    {
        sceneHandle = YooAssets.LoadSceneAsync("Game");
        sceneHandle.Completed += (handle) =>
        {
            handle.ActivateScene();
        };
        yield return sceneHandle;
    }
    IEnumerator LoadUIManager()
    {
        AssetHandle _handle = YooAssets.LoadAssetAsync<GameObject>("UIManager");
        _handle.Completed += (handle) =>
        {
            GameObject go = Instantiate((GameObject)_handle.AssetObject);
            DontDestroyOnLoad(go);
            Debug.Log(_handle.AssetObject);
        };
        yield return _handle;
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneHandle!=null && !sceneHandle.IsDone)
        {
            Debug.Log(sceneHandle.Progress);
        }
    }
}
