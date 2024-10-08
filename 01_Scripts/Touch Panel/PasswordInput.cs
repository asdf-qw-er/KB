using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PasswordInput : MonoBehaviour
{
    [Header("Number Pad")]
    [SerializeField] private Text m_Txt_Password;
    [SerializeField] private Button[] m_Btn_NumPad;
    [SerializeField] private Button m_Btn_Backspace;
    [SerializeField] private Button m_Btn_Reset;

    [Header("Screen")]
    [SerializeField] private GameObject m_Screen_PasswordInput;
    [SerializeField] private GameObject m_Screen_Termination;

    #region Life Cycle
    void Start() { Listeners(); }

    void OnEnable() { OnClickReset(); }
    #endregion

    #region Private
    private void Listeners()
    {
        for (int i = 0; i < m_Btn_NumPad.Length; i++)
        {
            int _i = i;
            m_Btn_NumPad[_i].onClick.AddListener(OnClickNumberPad);
        }

        m_Btn_Backspace.onClick.AddListener(OnClickBackspace);
        m_Btn_Reset.onClick.AddListener(OnClickReset);
    }

    private void OnClickNumberPad()
    {
        m_Txt_Password.text += "*";
        if (m_Txt_Password.text.Length == 4)
            StartCoroutine(ScreenControl());
    }

    private void OnClickBackspace()
    {
        if (m_Txt_Password.text.Length != 0) {
            int len = m_Txt_Password.text.Length - 1;
            m_Txt_Password.text = "";

            for (int i = 0; i < len; i++)
                m_Txt_Password.text += "*";
        }
    }

    private void OnClickReset()
    {
        m_Txt_Password.text = "";
    }

    private IEnumerator ScreenControl()
    {
        yield return new WaitForSeconds(1f);
        m_Screen_PasswordInput.SetActive(false);
        m_Screen_Termination.SetActive(true);
    }
    #endregion

    #region Public
    #endregion
}
