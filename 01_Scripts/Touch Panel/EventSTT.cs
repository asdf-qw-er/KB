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
            case "�������":
            case "���":
            case "���":
            case "����":
                buttonIdx = 0; break;
            case "�Ա�":
                buttonIdx = 1; break;
            case "���¼۱�":
            case "�۱�":
            case "����":
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
