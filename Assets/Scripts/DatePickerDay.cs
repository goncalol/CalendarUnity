using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DatePickerDay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public DatePicker DatePicker;
    private TextMeshProUGUI text;
    private DateTime date;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontSize = 30;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontSize = 23.1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        DatePicker.OnDayClicked(date);
    }

    internal void SetDate(DateTime currentDate, Color color)
    {
        date = currentDate;
        text.SetText(date.Day.ToString());
        text.color = color;
    }
}
