using UnityEngine;
using UnityEngine.UI;

public class AccountInput : MonoBehaviour
{
    [Header("Number Pad")]
    [SerializeField] private Text m_Txt_Account;
    [SerializeField] private Button[] m_Btn_NumPad; // 1..9, 0
    [SerializeField] private Button m_Btn_Backspace;
    [SerializeField] private Button m_Btn_Reset;

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
            m_Btn_NumPad[_i].onClick.AddListener(delegate { OnClickNumberPad(_i); });
        }

        m_Btn_Backspace.onClick.AddListener(OnClickBackspace);
        m_Btn_Reset.onClick.AddListener(OnClickReset);
    }

    private void OnClickNumberPad(int num)
    {
        m_Txt_Account.text += num;
    }

    private void OnClickBackspace()
    {
        if (m_Txt_Account.text.Length != 0)
            m_Txt_Account.text = m_Txt_Account.text.Remove(m_Txt_Account.text.Length - 1, 1);
    }

    private void OnClickReset()
    {
        m_Txt_Account.text = "";
    }
    #endregion

    #region Public
    #endregion
}
