using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class DatePicker : MonoBehaviour
{
    public GameObject Grid;
    public TextMeshProUGUI Title;
    public PopupForm InsertForm;
    public TMP_InputField HourInput;
    public TMP_InputField MinuteInput;
    private DateTime currentDate;
    void Start()
    {
        currentDate = DateTime.UtcNow;
        ChangeCalendar(currentDate);
    }

    private void ChangeCalendar(DateTime date)
    {
        Title.SetText(string.Format("{0} {1}", date.ToString("MMMM"), date.Year));

        var ca = CultureInfo.InvariantCulture.Calendar;
        var firstDayOfCurrentMonth = new DateTime(date.Year, date.Month, 1);
        var firstDayCellIndex = (int)ca.GetDayOfWeek(firstDayOfCurrentMonth);

        var currentDateNotAltered = firstDayOfCurrentMonth;
        var currentDate = firstDayOfCurrentMonth;
        for (var i = firstDayCellIndex; i < 7; i++)
        {
            var c = Grid.transform.GetChild(i).GetComponent<DatePickerDay>();
            c.SetDate(currentDate,Color.black);
            currentDate = currentDate.AddDays(1);
        }

        var lastDayPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
        for (var i = firstDayCellIndex - 1; i >= 0; i--)
        {
            var c = Grid.transform.GetChild(i).GetComponent<DatePickerDay>();
            c.SetDate(lastDayPreviousMonth, Color.gray);
            lastDayPreviousMonth = lastDayPreviousMonth.AddDays(-1);
        }

        for (var i = 7; i <= 41; i++)
        {
            var c = Grid.transform.GetChild(i).GetComponent<DatePickerDay>();
            if (currentDate.Month != currentDateNotAltered.Month)
            {
                c.SetDate(currentDate, Color.gray);
            }
            else
            {
                c.SetDate(currentDate, Color.black);
            }
            currentDate = currentDate.AddDays(1);
        }

    }

    internal void OnDayClicked(DateTime date)
    {
        if (!string.IsNullOrEmpty(HourInput.text))
        {
            var i = int.Parse(HourInput.text);
            if(i>=0 && i <= 24)
            {
                date = date.AddHours(i);
            }
        }
        if (!string.IsNullOrEmpty(MinuteInput.text))
        {
            var i = int.Parse(MinuteInput.text);
            if (i >= 0 && i <= 60)
            {
                date = date.AddMinutes(i);
            }
        }
        InsertForm.SetDate(date);
        CloseDatePicker();
    }

    public void NextMonth()
    {
        currentDate = currentDate.AddMonths(1);
        ChangeCalendar(currentDate);
    }

    public void PrevMonth()
    {
        currentDate = currentDate.AddMonths(-1);
        ChangeCalendar(currentDate);
    }


    public void CloseDatePicker()
    {
        transform.gameObject.SetActive(false);
    }
}
