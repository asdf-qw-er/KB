using System;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private PythonManager m_PythonManager;

    #region Life Cycle
    void OnEnable() { RunOD(); }

    void OnDisable() { RunED(); }
    #endregion

    #region Private
    private void RunED()
    {
        try { m_PythonManager.ObjectDetection.Kill(); } catch (Exception) { }
        m_PythonManager.Run("ED.py");
    }

    private void RunOD()
    {
        try { m_PythonManager.EmotionDetection.Kill(); } catch (Exception) { }
        //m_PythonManager.Run("OD.py");
    }
    #endregion
}
