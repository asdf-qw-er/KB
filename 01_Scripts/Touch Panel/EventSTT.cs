using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventSTT : MonoBehaviour
{
    [SerializeField] private Server m_Server;
    [SerializeField] private Button[] m_Button;

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
        switch (m_Server.STT.Replace(" ", ""))
        {
            case "예금출금":
            case "출근":
            case "출금":
            case "예금":
                buttonIdx = 0; break;
            case "입금":
                buttonIdx = 1; break;
            case "계좌송금":
            case "송금":
            case "계좌":
                buttonIdx = 2; break;
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
