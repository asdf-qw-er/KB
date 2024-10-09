using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server : MonoBehaviour
{
    #region TCP
    private TcpListener server;
    private const int port = 5555;
    #endregion

    #region STT
    public string STT;
    public bool isChangedSTT;
    #endregion

    #region Emotion
    public int Emotion;
    #endregion

    #region Life Cycle
    void Start() { StartServer(); }

    void OnApplicationQuit() { KillProcess(); }
    #endregion

    #region Private
    private void StartServer()
    {
        server = new TcpListener(IPAddress.Any, port);
        server.Start();
        Debug.LogError("Method <StartServer>: Server started.");

        AcceptClients();
    }

    private void KillProcess()
    {
        server.Stop();
        Debug.LogError("Method <KillProcess>: Server stopped.");
    }

    private async void AcceptClients()
    {
        while (true)
        {
            TcpClient client = await server.AcceptTcpClientAsync();
            Debug.LogError("Method <AcceptClients>: Client connected.");
            ProcessClient(client);
        }
    }

    private async void ProcessClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();

        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                HandlePacket(buffer, bytesRead);
        }
        catch (Exception ex)
        {
            Debug.LogError("Method <ProcessClient> Error: " + ex.Message);
        }
        finally
        {
            client.Close();
            Debug.LogError("Method <ProcessClient> Error: Client disconnected.");
        }
    }

    private void HandlePacket(byte[] packet, int length)
    {
        if (length < 5)
            return;

        //byte startByte = packet[0];
        byte ID = packet[1];
        byte PID = packet[2];
        byte messageLength = packet[3];

        if (length < 4 + messageLength + 1)
            return;

        byte[] messageBytes = new byte[messageLength];
        Array.Copy(packet, 4, messageBytes, 0, messageLength);

        // PID(01) : STT
        if (ID == 0x02 && PID == 0x01)
        {
            try 
            {
                STT = Encoding.UTF8.GetString(messageBytes);
                isChangedSTT = true;

                //Debug.Log("STT received: " + STT);
            }
            catch (Exception) { }
        }
        // PID(02) : Emotion Detection
        else if (ID==0x02 && PID == 0x02)
        {
            Emotion = messageBytes[0];

            //Debug.Log("Emotion received: " + Emotion);
        }

        //byte receivedChecksum = packet[4 + messageLength];
        //byte calculatedChecksum = CalculateChecksum(startByte, ID, PID, messageLength);
        //if (receivedChecksum != calculatedChecksum)
        //    return;
    }

    private byte CalculateChecksum(byte startByte, byte version, byte messageType, byte messageLength)
    {
        int checksum = ~(startByte + version + messageType) + 1;
        return (byte)(checksum & 0xFF);
    }
    #endregion

    #region Public
    #endregion
}
