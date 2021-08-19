using Assets.Scripts.Messages;
using TMPro;
using UnityEngine;

public class EventListItem : MonoBehaviour
{
    public TextMeshProUGUI Title;
    private Calendar calendar;
    private CalendarEvent calendarEvent;
    private GameObject LoadingPanel;

    private void Awake()
    {
        calendar = GameObject.FindGameObjectWithTag("Calendar").GetComponent<Calendar>();
        LoadingPanel = calendar.LoadingPanel;
    }

    public void SetItem(CalendarEvent t)
    {
        calendarEvent = t;
        Title.SetText(t.title);
    }

    public void OnDeleteClick()
    {
        StartCoroutine(calendar.RemoveEvent(calendarEvent));
        LoadingPanel.SetActive(true);
    }
}
