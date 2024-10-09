using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class PythonManager : MonoBehaviour
{
    public Process EmotionDetection;
    public Process ObjectDetection;
    public Process STT;
    public Process TTS;

    void Awake()
    {
        Run("TTS.py");
        Run("STT.py");
    }

    void OnDisable() { KillProcess(); }

    void OnDestroy() { KillProcess(); }

    void OnApplicationQuit() { KillProcess(); }

    #region Public
    public void Run(string scriptName)
    {
        string scriptPath = Path.Combine(Application.persistentDataPath, "Python", scriptName);

        ProcessStartInfo start = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = $"\"{scriptPath}\"",
            WorkingDirectory = Path.GetDirectoryName(scriptPath),
            UseShellExecute = false,
            CreateNoWindow = true,

            RedirectStandardInput = true,
        };

        try
        {
            switch (scriptName)
            {
                case "ED.py":
                    EmotionDetection = Process.Start(start);
                    break;
                case "OD.py":
                    ObjectDetection = Process.Start(start);
                    break;
                case "STT.py":
                    STT = Process.Start(start);
                    break;
                case "TTS.py":
                    TTS = Process.Start(start);
                    break;
            }
        }
        catch (Exception) { }
    }

    public void KillProcess()
    {
        try { EmotionDetection.Kill(); } catch (Exception) { }
        try { ObjectDetection.Kill(); } catch (Exception) { }
        try { STT.Kill(); } catch (Exception) { }
        try { TTS.Kill(); } catch (Exception) { }
    }
    #endregion
}
