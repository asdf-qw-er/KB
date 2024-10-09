using UnityEngine;
using UnityEngine.UI;

public class EventSTT : MonoBehaviour
{
    [SerializeField] private Server m_Server;
    // Length of m_Button must be same as Length of m_STT
    [SerializeField] private Button[] m_Button;
    [SerializeField] private string[] m_STT;

    #region Life Cycle
    void Update()
    {
        if (m_Server.isChangedSTT)
            OnSTTChanged();
    }
    #endregion

    #region Private
    private void OnSTTChanged()
    {
        Debug.Log("Method <OnSTTChanged>: " + m_Server.STT);

        int buttonIdx = -1;

        string processedSTT = m_Server.STT.Replace(" ", "");
        for (int i = 0; i < m_Button.Length; i++)
        {
            string[] keywords = m_STT[i].Split(";");
            foreach (string keyword in keywords)
            {
                if (processedSTT == keyword)
                {
                    buttonIdx = i;
                    break;
                }
            }
        }

        if (buttonIdx != -1)
            if (m_Button[buttonIdx].gameObject.activeSelf)
                m_Button[buttonIdx].onClick.Invoke();

        m_Server.isChangedSTT = false;
    }
    #endregion

    #region Public
    #endregion
}
