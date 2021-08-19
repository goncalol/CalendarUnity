using Assets.Scripts.Messages;
using TMPro;
using UnityEngine;

public class PopupEventDetail : MonoBehaviour
{
    public TextMeshProUGUI PopupTitle;
    public Calendar Calendar;
    public GameObject LoadingPanel;

    private CalendarEvent calendarEvent;

    internal void SetChosenEvent(CalendarEvent calendarEvent)
    {
        this.calendarEvent = calendarEvent;
        PopupTitle.SetText(calendarEvent.title);
    }

    public void OnRemoveClick()
    {
        StartCoroutine(Calendar.RemoveEvent(calendarEvent));
        LoadingPanel.SetActive(true);
    }
}
