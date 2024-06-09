using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [SerializeField] TMP_InputField playerName;
    [SerializeField] GameObject Input;
    [SerializeField] TextMeshProUGUI ErrorMsg;
    public void OnClick_Submit()
    {
        string name = playerName.text;
        TcpSender.Instance.OnSetMyName(name);
        if(TcpSender.Instance.ConnectToServer())
        {
            Input.gameObject.SetActive(false);
            ErrorMsg.gameObject.SetActive(true);
            ErrorMsg.text = "상대를 찾는중입니다.";
        }
        else
        {
            ErrorMsg.gameObject.SetActive(true);
        }
    }

    public void LoadScene()
    {
        StartCoroutine(GameStart());
    }

    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(1);
        ErrorMsg.gameObject.SetActive(true);
        ErrorMsg.text = "상대를 찾았습니다.\n잠시후 게임이 시작합니다.";
        yield return new WaitForSeconds(2);
        ErrorMsg.text = "5";
        yield return new WaitForSeconds(1);
        ErrorMsg.text = "4";
        yield return new WaitForSeconds(1);
        ErrorMsg.text = "3";
        yield return new WaitForSeconds(1);
        ErrorMsg.text = "2";
        yield return new WaitForSeconds(1);
        ErrorMsg.text = "1";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("SampleScene");

    }
}
