using System;
using UnityEngine;
using UnityEngine.UI;

public class SetDisplayPanelTime : MonoBehaviour
{
    [Header("Time")]
    [SerializeField] private Text m_Txt_Date;
    [SerializeField] private Text m_Txt_Month;
    [SerializeField] private Text m_Txt_Year;
    [SerializeField] private Text m_Txt_Day;

    #region Life Cycle
    void Update() { SetTimeText(); }
    #endregion

    #region Private
    private void SetTimeText()
    {
        DateTime Now = DateTime.Now;

        m_Txt_Date.text = string.Format("{0:00}", Now.Day);
        m_Txt_Month.text = GetEngMonth(Now);
        m_Txt_Year.text = Now.Year.ToString();
        m_Txt_Day.text = GetKorDay(Now);
    }

    private string GetEngMonth(DateTime Now)
    {
        switch (Now.Month)
        {
            case 1:
                return "JAN";
            case 2:
                return "FEB";
            case 3:
                return "MAR";
            case 4:
                return "APR";
            case 5:
                return "MAY";
            case 6:
                return "JUN";
            case 7:
                return "JUL";
            case 8:
                return "AUG";
            case 9:
                return "SEP";
            case 10:
                return "OCT";
            case 11:
                return "NOV";
            default:
                return "DEC";
        }
    }

    private string GetKorDay(DateTime Now)
    {
        switch (Now.DayOfWeek)
        {
            case DayOfWeek.Monday:
                return "월요일";
            case DayOfWeek.Tuesday:
                return "화요일";
            case DayOfWeek.Wednesday:
                return "수요일";
            case DayOfWeek.Thursday:
                return "목요일";
            case DayOfWeek.Friday:
                return "금요일";
            case DayOfWeek.Saturday:
                return "토요일";
            default:
                return "일요일";
        }
    }
    #endregion

    #region Public
    #endregion
}
