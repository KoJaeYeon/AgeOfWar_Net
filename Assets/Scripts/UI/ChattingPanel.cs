using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatLog;
    [SerializeField] TMP_InputField ChatInputField;

    string myName = string.Empty;

    private void Awake()
    {
        TcpSender.Instance.chattingPanel = this;
        myName = TcpSender.Instance.GetMyName();
    }
    public void OnChatLogWrite(string msg)
    {        
        ChatLog.text += msg;
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
