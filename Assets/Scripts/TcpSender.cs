using System;
using System.Collections;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class TcpSender : MonoBehaviour
{
    TcpClient client;
    NetworkStream stream;
    [SerializeField] TextMeshProUGUI chatLog;
    [SerializeField] TMP_InputField inputField;
    string server = "127.0.0.1";
    int port = 13000;
    bool isConnected = false;

    private void Start()
    {
        ConnectToServer();
    }
    public void ConnectToServer()
    {
        if (isConnected) { return; }
        try
        {
            client = new TcpClient(server, port);
            isConnected = true;
            stream = client.GetStream();

            // 수신 스레드 시작
            StartCoroutine(ReceiveData());
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to connect to server: " + e.Message);
        }
    }

    public void OnClick_SendMsg()
    {
        string msg = inputField.text;
        if (string.IsNullOrWhiteSpace(msg))
        {
            msg = "null";
        }
        SendMsg(msg);
    }

    private void SendMsg(String message)
    {
        try
        {
            if (client == null)
            {
                Debug.Log("ConnectNeed");
                return;
            }
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            stream = client.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);

            // Receive the server response.

            // Buffer to store the response bytes.
            data = new Byte[256];

            // String to store the response ASCII representation.
            String responseData = String.Empty;

            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Debug.Log($"Received: {responseData}");
            chatLog.text += responseData;
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine("ArgumentNullException: {0}", e);
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
    }

    IEnumerator ReceiveData()
    {
        while (isConnected)
        {
            try
            {
                if (client.Available > 0)
                {
                    stream = client.GetStream();

                    // 서버로부터 데이터 읽기
                    byte[] buffer = new byte[client.Available];
                    stream.Read(buffer, 0, buffer.Length);

                    // 받은 데이터 처리
                    string receivedMessage = System.Text.Encoding.ASCII.GetString(buffer);
                    Debug.Log("Received: " + receivedMessage);
                    chatLog.text += receivedMessage;
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
            }

            // 0.1초 대기 후 다시 확인
            yield return new WaitForSeconds(0.1f);
        }
    }

    void OnDestroy()
    {
        Disconnect();
    }

    public void Disconnect()
    {
        isConnected = false;

        if (stream != null)
            stream.Close();

        if (client != null)
            client.Close();
    }
}
