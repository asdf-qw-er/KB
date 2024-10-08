using UnityEngine;
using UnityEngine.UI;

public class DistributionBills : MonoBehaviour
{
    [Header("Distribution Bills")]
    [SerializeField] private Text m_Txt_TotalAmount;
    [SerializeField] private Text m_Txt_Checkamount;
    [SerializeField] private Text m_Txt_50000;
    [SerializeField] private Text m_Txt_10000;

    public int m_Bills;
    public bool m_isCheck;

    #region Life Cycle
    void OnEnable() { SetBillsText(); }
    #endregion

    #region Private
    private void SetBillsText()
    {
        m_Txt_TotalAmount.text = string.Format("<color=#1400FF>{0}</color> 만원", m_Bills);
        if (m_isCheck)
        {
            m_Txt_Checkamount.text = string.Format("<color=#1400FF>{0}</color> 만원", m_Bills);
            m_Txt_50000.text = "<color=#1400FF>0</color> 만원";
            m_Txt_10000.text = "<color=#1400FF>0</color> 만원";
        }
        else
        {
            m_Txt_Checkamount.text = "<color=#1400FF>0</color> 만원";
            m_Txt_50000.text = string.Format("<color=#1400FF>{0}</color> 만원", m_Bills / 5 * 5);
            m_Txt_10000.text = string.Format("<color=#1400FF>{0}</color> 만원", m_Bills % 5);
        }
    }
    #endregion

    #region Public
    public void SetBills(int Bills)
    {
        m_Bills = Bills;
    }

    public void SetIsCheck(bool isCheck)
    {
        m_isCheck = isCheck;
    }
    #endregion
}
