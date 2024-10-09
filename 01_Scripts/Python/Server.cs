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
    public byte Emotion;
    #endregion

    #region Object
    public byte[] Object;
    public enum ObjectCategory
    {
        Person,               // 0
        Bicycle,              // 1
        Car,                  // 2
        Motorcycle,           // 3
        Airplane,             // 4
        Bus,                  // 5
        Train,                // 6
        Truck,                // 7
        Boat,                 // 8
        TrafficLight,         // 9
        FireHydrant,          // 10
        StopSign,             // 11
        ParkingMeter,         // 12
        Bench,                // 13
        Bird,                 // 14
        Cat,                  // 15
        Dog,                  // 16
        Horse,                // 17
        Sheep,                // 18
        Cow,                  // 19
        Elephant,             // 20
        Bear,                 // 21
        Zebra,                // 22
        Giraffe,              // 23
        Backpack,             // 24
        Umbrella,             // 25
        Handbag,              // 26
        Tie,                  // 27
        Suitcase,             // 28
        Frisbee,              // 29
        Skis,                 // 30
        Snowboard,            // 31
        SportsBall,           // 32
        Kite,                 // 33
        BaseballBat,          // 34
        BaseballGlove,        // 35
        Skateboard,           // 36
        Surfboard,            // 37
        TennisRacket,         // 38
        Bottle,               // 39
        WineGlass,            // 40
        Cup,                  // 41
        Fork,                 // 42
        Knife,                // 43
        Spoon,                // 44
        Bowl,                 // 45
        Banana,               // 46
        Apple,                // 47
        Sandwich,             // 48
        Orange,               // 49
        Broccoli,             // 50
        Carrot,               // 51
        HotDog,               // 52
        Pizza,                // 53
        Donut,                // 54
        Cake,                 // 55
        Chair,                // 56
        Couch,                // 57
        PottedPlant,          // 58
        Bed,                  // 59
        DiningTable,          // 60
        Toilet,               // 61
        TV,                   // 62
        Laptop,               // 63
        Mouse,                // 64
        Remote,               // 65
        Keyboard,             // 66
        CellPhone,            // 67
        Microwave,            // 68
        Oven,                 // 69
        Toaster,              // 70
        Sink,                 // 71
        Refrigerator,         // 72
        Book,                 // 73
        Clock,                // 74
        Vase,                 // 75
        Scissors,             // 76
        TeddyBear,            // 77
        HairDrier,            // 78
        Toothbrush            // 79
    }
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
        else if (ID == 0x02 && PID == 0x02)
        {
            Emotion = messageBytes[0];

            //Debug.Log("Emotion received: " + Emotion);
        }
        // PID(03) : Object Detection
        else if (ID == 0x02 && PID == 0x03)
        {
            Object = messageBytes;

            //string sObject = string.Empty;
            //for (int i = 0; i < Object.Length; i++)
            //    sObject += (ObjectCategory)Object[i] + " ";
            //Debug.Log("Object received: " + sObject);
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
