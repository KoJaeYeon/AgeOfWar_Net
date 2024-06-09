using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChattingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatLog;
    [SerializeField] TMP_InputField ChatInputField;
    [SerializeField] Scrollbar Scrollbar;

    string myName = string.Empty;

    private void Awake()
    {
        TcpSender.Instance.chattingPanel = this;
        myName = TcpSender.Instance.GetMyName();
    }
    public void OnChatLogWrite(string msg)
    {
        StartCoroutine(AppendAndScroll(msg));
    }

    IEnumerator AppendAndScroll(string msg)
    {
        ChatLog.text += msg;

        yield return null;
        yield return null;

        Scrollbar.value = 0;
    }
    public void OnClick_SendMsg()
    {
        if(string.IsNullOrWhiteSpace(ChatInputField.text))
        {
            return;
        }

        string msg = ChatInputField.text;
        ChatInputField.text = string.Empty;
        Debug.Log(msg);
        OnChatLogWrite($"<color=red>{myName}</color> : {msg}\n");

        TcpSender.Instance.SendMsg(msg);
    }
}
