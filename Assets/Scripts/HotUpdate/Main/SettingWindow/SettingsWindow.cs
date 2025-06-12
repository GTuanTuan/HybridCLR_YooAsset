using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [Header("Panels")]
    public GameObject generalPanel;
    public GameObject audioPanel;
    public GameObject graphicsPanel;
    public GameObject displayPanel;

    [Header("Tab Buttons")]
    public Button generalTab;
    public Button audioTab;
    public Button graphicsTab;
    public Button displayTab;

    [Header("Action Buttons")]
    public Button applyButton;
    public Button cancelButton;
    public Button defaultsButton;
    public Button quitButton;

    private GameObject currentPanel;

    private void Awake()
    {
        // 初始化标签页按钮
        generalTab.onClick.AddListener(() => SwitchPanel(generalPanel));
        audioTab.onClick.AddListener(() => SwitchPanel(audioPanel));
        graphicsTab.onClick.AddListener(() => SwitchPanel(graphicsPanel));
        displayTab.onClick.AddListener(() => SwitchPanel(displayPanel));

        // 初始化动作按钮
        applyButton.onClick.AddListener(ApplyAllSettings);
        cancelButton.onClick.AddListener(CloseWindow);
        defaultsButton.onClick.AddListener(ResetToDefaults);
        quitButton.onClick.AddListener(() => { Application.Quit(); });

        // 默认打开第一个面板
        SwitchPanel(generalPanel);
    }

    private void SwitchPanel(GameObject newPanel)
    {
        if (currentPanel != null)
            currentPanel.SetActive(false);

        newPanel.SetActive(true);
        currentPanel = newPanel;
        UpdateTabButtons();
    }

    private void UpdateTabButtons()
    {
        generalTab.interactable = currentPanel != generalPanel;
        audioTab.interactable = currentPanel != audioPanel;
        graphicsTab.interactable = currentPanel != graphicsPanel;
        displayTab.interactable = currentPanel != displayPanel;
    }

    public void ApplyAllSettings()
    {
        // 应用显示设置（需要最先执行）
        displayPanel.GetComponent<DisplayPanel>().ApplySettings();

        // 应用图形设置
        graphicsPanel.GetComponent<GraphicsPanel>().ApplySettings();

        // 应用音频设置
        audioPanel.GetComponent<AudioPanel>().ApplySettings();

        // 保存所有设置
        SettingsManager.Instance.SaveSettings();

        CloseWindow();
    }

    public void CloseWindow()
    {
        // 重新加载设置以取消未保存的更改
        SettingsManager.Instance.LoadSettings();
        gameObject.SetActive(false);
    }
    public void ResetToDefaults()
    {
        SettingsManager.Instance.ResetToDefaultSettings();
        SwitchPanel(currentPanel); // 刷新当前面板
    }
}