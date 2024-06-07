using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ChatLog;
    [SerializeField] TMP_InputField ChatInputField;
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
        OnChatLogWrite($"<color=red>³ª</color> : {msg}\n");

        TcpSender.Instance.SendMsg(msg);
    }
}
