using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private Text titleText;
    [SerializeField] private Text contentText;
    [SerializeField] private Transform buttonsParent;
    [SerializeField] private GameObject buttonPrefab;

    private static readonly List<MessageBox> hiddenMessageBoxes = new List<MessageBox>();
    private readonly List<Button> createdButtons = new List<Button>();

    public static MessageBox Show()
    {

        if (hiddenMessageBoxes.Count > 0)
        {
            MessageBox box = hiddenMessageBoxes[0];
            hiddenMessageBoxes.RemoveAt(0);
            box.gameObject.SetActive(true);
            box.panel.SetActive(true);
            return box;
        }

        var prefab = Resources.Load<GameObject>("MessageBox");
        if (!prefab)
        {
            Debug.LogError("MessageBox prefab not found in Resources");
            return null;
        }

        var go = Instantiate(prefab, GameManager.Inst.MainUICanvas.transform);
        go.name = "MessageBox";
        var messageBox = go.GetComponent<MessageBox>();
        return messageBox;
    }

    public void Hide()
    {
        panel.SetActive(false);
        ClearButtons();
        gameObject.SetActive(false);
        hiddenMessageBoxes.Add(this); 
    }

    private void ClearButtons()
    {
        foreach (var button in createdButtons)
        {
            if (button != null) Destroy(button.gameObject);
        }
        createdButtons.Clear();
    }

    public MessageBox SetTitle(string title)
    {
        titleText.text = title;
        return this;
    }

    public MessageBox SetContent(string content)
    {
        contentText.text = content;
        return this;
    }

    public MessageBox AddButton(string text, Action<MessageBox> onClick = null)
    {
        var button = Instantiate(buttonPrefab, buttonsParent).GetComponent<Button>();
        button.GetComponentInChildren<Text>().text = text;

        button.onClick.AddListener(() =>
        {
            onClick?.Invoke(this);
            Hide();
        });

        createdButtons.Add(button);
        button.gameObject.SetActive(true);
        return this;
    }
}