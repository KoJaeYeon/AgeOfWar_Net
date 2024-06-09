using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] GameObject ErrorMsg;
    public void OnClick_Submit()
    {
        string name = playerName.text;
        TcpSender.Instance.OnSetMyName(name);
        if(TcpSender.Instance.ConnectToServer())
        {
            gameObject.SetActive(false);
        }
        else
        {
            ErrorMsg.SetActive(true);
        }
    }
}
