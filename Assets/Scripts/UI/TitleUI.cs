using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    public void OnClick_Submit()
    {
        string name = playerName.text;
        TcpSender.Instance.OnSetMyName(name);
        TcpSender.Instance.ConnectToServer();
    }
}
