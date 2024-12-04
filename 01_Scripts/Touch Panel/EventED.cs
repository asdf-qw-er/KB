using UnityEngine;

public class EventED : MonoBehaviour
{
    [SerializeField] private Server m_Server;
    [SerializeField] private GameObject m_Popup;

    public const int LIMIT = 3;

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
            (m_Server.Emotion == 0x01 || m_Server.Emotion == 0x02 || m_Server.Emotion == 0x04 || m_Server.Emotion == 0x05))
        {
            m_Time += Time.deltaTime;
            if (m_Time >= LIMIT)
            {
                if (!m_Popup.activeSelf)
                {
                    m_Popup.SetActive(true);
                    isPopupActivated = true;
                }
            }
        }
        else
            m_Time = 0f;
    }

    private void _OnEnable()
    {
        isPopupActivated = false;
        m_Time = 0f;

        m_Popup.SetActive(false);
    }
    #endregion

    #region Public
    #endregion
}
