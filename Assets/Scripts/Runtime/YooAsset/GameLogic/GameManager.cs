using HybridCLR;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using YooAsset;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class GameManager
{
    public MonoBehaviour Behaviour;
    public string ServerAddress;
    public GameObject MainUICanvas;
    public Camera UICamera;

    private static GameManager _instance;
    public static GameManager Inst
    {
        get
        {
            if (_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    public void StartCoroutine(IEnumerator enumerator)
    {
        Behaviour.StartCoroutine(enumerator);
    }

    public void LoadDll(ResourcePackage package, string dll)
    {
        if (package.GetAssetInfo(dll).Error == string.Empty)
        {
            AssetHandle handle = package.LoadAssetSync<TextAsset>(dll);
#if UNITY_EDITOR
            Assembly hotUpdateAss = System.AppDomain.CurrentDomain.GetAssemblies().First(a => a.GetName().Name == dll.Replace(".dll", ""));
#else
            Assembly hotUpdateAss = Assembly.Load((handle.AssetObject as TextAsset).bytes);
#endif
            Debug.Log($"加载{dll}");
        }
    }
    public void LoadDepDll(ResourcePackage package,List<string> dlls)
    {
        foreach (string dll in dlls)
        {
            if (package.GetAssetInfo(dll).Error == string.Empty)
            {
                AssetHandle handle = package.LoadAssetSync<TextAsset>(dll);
                RuntimeApi.LoadMetadataForAOTAssembly((handle.AssetObject as TextAsset).bytes, HomologousImageMode.SuperSet);
            }
        }   
    }
    public void LoadServerSettings()
    {
        string settingsPath = Path.Combine(Application.persistentDataPath, "settings.json");
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            ServerAddress = JsonUtility.FromJson<ServerSettings>(json).serverAddress;
            Debug.Log(ServerAddress);
        }
    }
    [System.Serializable]
    public class ServerSettings
    {
        public string serverAddress;
    }
}