using UnityEngine;
using System.IO;
using YooAsset;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    private GameSettings _currentSettings;
    private SettingsWindow settingsWindow;
    public GameSettings CurrentSettings => _currentSettings;
    private string settingsPath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            Debug.Log("创建多个");
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        settingsPath = Path.Combine(Application.persistentDataPath, "settings.json");
        LoadSettings();
    }

    public void LoadSettings()
    {
        if (File.Exists(settingsPath))
        {
            string json = File.ReadAllText(settingsPath);
            _currentSettings = JsonUtility.FromJson<GameSettings>(json);
        }
        else
        {
            ResetToDefaultSettings();
        }
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(_currentSettings, true);
        File.WriteAllText(settingsPath, json);
    }
    // 新增方法：重置为默认设置
    public void ResetToDefaultSettings()
    {
        _currentSettings = new GameSettings();
        SaveSettings();
    }
    public void OpenSettingWindow()
    {
        if (settingsWindow != null)
        {
            settingsWindow.gameObject.SetActive(true);
        }
        else
        {
            YooAssets.LoadAssetAsync<GameObject>("SettingsWindow").Completed += (handle) =>
            {
                settingsWindow = GameObject.Instantiate((GameObject)handle.AssetObject, GameManager.Inst.MainUICanvas.transform).GetComponent<SettingsWindow>();
            };
        }
    }
    public void CloseSettingWindow()
    {
        if (settingsWindow != null)
        {
            settingsWindow.gameObject.SetActive(false);
        }
    }
}

[System.Serializable]
public class GameSettings
{
    // 常规设置
    public string serverAddress = "http://127.0.0.1:8080";

    // 音频设置
    public float masterVolume = 0.75f;

    // 图形设置
    public int qualityLevel = 2;
    public bool vsyncEnabled = true;
    public float shadowDistance = 50f;
    public int antiAliasing = 2;
    public bool bloomEnabled = true;
    public float renderScale = 1.0f;

    // 显示设置
    public int resolutionIndex = 0;
    public bool fullscreen = true;
    public bool borderless = false;
    public int displayIndex = 0;
}