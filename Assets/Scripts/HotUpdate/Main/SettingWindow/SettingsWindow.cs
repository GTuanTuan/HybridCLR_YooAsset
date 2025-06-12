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
        // ��ʼ����ǩҳ��ť
        generalTab.onClick.AddListener(() => SwitchPanel(generalPanel));
        audioTab.onClick.AddListener(() => SwitchPanel(audioPanel));
        graphicsTab.onClick.AddListener(() => SwitchPanel(graphicsPanel));
        displayTab.onClick.AddListener(() => SwitchPanel(displayPanel));

        // ��ʼ��������ť
        applyButton.onClick.AddListener(ApplyAllSettings);
        cancelButton.onClick.AddListener(CloseWindow);
        defaultsButton.onClick.AddListener(ResetToDefaults);
        quitButton.onClick.AddListener(() => { Application.Quit(); });

        // Ĭ�ϴ򿪵�һ�����
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
        // Ӧ����ʾ���ã���Ҫ����ִ�У�
        displayPanel.GetComponent<DisplayPanel>().ApplySettings();

        // Ӧ��ͼ������
        graphicsPanel.GetComponent<GraphicsPanel>().ApplySettings();

        // Ӧ����Ƶ����
        audioPanel.GetComponent<AudioPanel>().ApplySettings();

        // ������������
        SettingsManager.Instance.SaveSettings();

        CloseWindow();
    }

    public void CloseWindow()
    {
        // ���¼���������ȡ��δ����ĸ���
        SettingsManager.Instance.LoadSettings();
        gameObject.SetActive(false);
    }
    public void ResetToDefaults()
    {
        SettingsManager.Instance.ResetToDefaultSettings();
        SwitchPanel(currentPanel); // ˢ�µ�ǰ���
    }
}