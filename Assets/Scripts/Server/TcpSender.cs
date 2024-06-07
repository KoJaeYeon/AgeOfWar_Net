using System;
using System.Collections;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class TcpSender : Singleton<TcpSender>
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
        ConnectToDB();
        //ConnectToServer();
    }

    public void ConnectToDB()
    {

    }

    public void ConnectToServer()
    {
        if (isConnected) { return; }
        try
        {
            client = new TcpClient(server, port);
            isConnected = true;
            stream = client.GetStream();

            // ���� ������ ����
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

    public void SendMsg(String message)
    {
        try
        {
            if (client == null)
            {
                Debug.Log("ConnectNeed");
                return;
            }
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(message);

            // Get a client stream for reading and writing.
            stream = client.GetStream();

            // Send the message to the connected TcpServer.
            stream.Write(data, 0, data.Length);

            Console.WriteLine("Sent: {0}", message);
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

                    // �����κ��� ������ �б�
                    byte[] buffer = new byte[client.Available];
                    stream.Read(buffer, 0, buffer.Length);

                    // ���� ������ ó��
                    string receivedMessage = System.Text.Encoding.UTF8.GetString(buffer);

                    ClassifyPacket(receivedMessage);

                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error receiving data: " + e.Message);
            }

            // 0.1�� ��� �� �ٽ� Ȯ��
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ClassifyPacket(string receivedMessage)
    {
        if (receivedMessage.Contains("[��ȯ]"))
        {
            string[] packets = receivedMessage.Split('/');
            SpawnManager.Instance.Spawn_Troop(int.Parse(packets[1]), TroopType.Enemy);
        }
        else
        {
            Debug.Log("Received: " + receivedMessage);
            chatLog.text += receivedMessage;
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
