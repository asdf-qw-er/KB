using UnityEngine;

public class EventOD : MonoBehaviour
{
    [SerializeField] private Server m_Server;
    [SerializeField] private GameObject m_Alert;
    [SerializeField] private GameObject m_Popup;

    private static bool isPersonDetected;
    private static bool isObjectDetected;
    private float m_Time = 0f;

    #region Life Cycle
    void Update() { GetStatus(); }
    #endregion

    #region Private
    private void GetStatus()
    {
        isPersonDetected = false;
        isObjectDetected = false;

        if (m_Server.Object.Length > 0)
        {
            for (int i = 0; i < m_Server.Object.Length; i++)
            {
                if (m_Server.Object[i] == 0)
                    isPersonDetected = true;
                else
                    isObjectDetected = true;
            }
        }

        if (isPersonDetected && isObjectDetected)
        {
            m_Alert.SetActive(true);
            m_Popup.SetActive(false);

            m_Time = 0f;
        }
        else if (!isPersonDetected && isObjectDetected) 
        {
            m_Time += Time.deltaTime;
            if(m_Time > 3f)
            {
                m_Alert.SetActive(true);
                m_Popup.SetActive(true);

                m_Time = 0f;
            }
        }
        else
        {
            m_Alert.SetActive(false);
            m_Popup.SetActive(false);

            m_Time = 0f;
        }
    }
    #endregion

    #region Public
    #endregion
}
