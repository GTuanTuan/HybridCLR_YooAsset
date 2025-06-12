using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPanel : MonoBehaviour
{
    [SerializeField] private TMP_InputField serverAddressInput;

    private void OnEnable()
    {
        serverAddressInput.text = SettingsManager.Instance.CurrentSettings.serverAddress;
    }

    public void OnServerAddressChanged(string value)
    {
        SettingsManager.Instance.CurrentSettings.serverAddress = value;
    }
}