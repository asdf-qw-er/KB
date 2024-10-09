using System;
using UnityEngine;
using UnityEngine.UI;

public class EventTTS : MonoBehaviour
{
    [SerializeField] private PythonManager m_PythonManager;
    [SerializeField] private bool isOnlyTTS;

    #region Life Cycle
    void Start()
    {
        if (!isOnlyTTS)
            gameObject.transform.GetComponent<Button>().onClick.AddListener(() => { GetTTS(gameObject.transform.GetChild(0).GetComponent<Text>().text.Replace(" ", "")); });
    }
    #endregion

    #region Private
    private void GetTTS(string temp)
    {
        try
        {
            if (m_PythonManager.TTS != null && !m_PythonManager.TTS.HasExited)
                m_PythonManager.TTS.StandardInput.WriteLine(temp);
            else
            {
                m_PythonManager.Run("TTS.py");
                m_PythonManager.TTS.StandardInput.WriteLine(temp);
            }
        }
        catch (Exception) { }
    }
    #endregion

    #region Public
    #endregion
}
