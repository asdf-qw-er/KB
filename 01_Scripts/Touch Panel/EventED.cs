using UnityEngine;

public class EventED : MonoBehaviour
{
    [SerializeField] private Server m_Server;
    [SerializeField] private GameObject m_Popup;

    public bool isPopupActivated;
    public float m_Time = 0f;

    // 'Angry', 'Disgust', 'Fear', 'Happy', 'Sad', 'Surprise', 'Neutral'
    // 0, 1, 2, 4

    #region Life Cycle
    void OnEnable() { _OnEnable(); }

    void Update() { GetNegativeEmotion(); }
    #endregion

    #region Private
    private void GetNegativeEmotion()
    {
        if (!isPopupActivated &&
            (m_Server.Emotion == 0x00 || m_Server.Emotion == 0x01 || m_Server.Emotion == 0x02 || m_Server.Emotion == 0x04))
        {
            m_Time += Time.deltaTime;
            if (m_Time >= 60)
            {
                if (!m_Popup.activeSelf)
                {
                    m_Popup.SetActive(true);
                    isPopupActivated = true;
                }
            }
        }
        else
            m_Time = 0;
    }

    private void _OnEnable()
    {
        isPopupActivated = false;
        m_Time = 0;

        m_Popup.SetActive(false);
    }
    #endregion

    #region Public
    #endregion
}
