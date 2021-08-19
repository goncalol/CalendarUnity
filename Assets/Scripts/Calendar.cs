using Assets.Scripts;
using Assets.Scripts.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class Calendar : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern string GetCSRFToken();

    [DllImport("__Internal")]
    private static extern void SendData(string str);

    private DateTime currentDate;
    private HttpClient client;
    private int firstDayCellIndex;
    private Transform canvas;
    private Transform grid;
    private DateTime firstDayOfCalendar;
    private DateTime lastDayOfCalendar;
    private Category[] categories;
    //private string accessToken;

    public TextMeshPro randomText;
    public GameObject InsertEventPopup;
    public GameObject LoadingPanel;
    public GameObject EventDetailPanel;
    public GameObject ListEventsPopup;
    public GameObject ListEventItem;

    private void Start()
    {
        canvas = transform.GetChild(0);
        grid = canvas.GetChild(1);
        currentDate = DateTime.UtcNow;

        ChangeCalendar(currentDate);
        StartCoroutine("GetEventData");
        LoadingPanel.SetActive(true);
    }

    IEnumerator GetEventData()
    {
        //UnityWebRequest www = UnityWebRequest.Get("https://localhost:44300/GetEvents");
        UnityWebRequest www = UnityWebRequest.Get("https://personalappv220210815170156.azurewebsites.net/GetEvents");
        //UnityWebRequest www = UnityWebRequest.Get("https://localhost:44307/Test");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            randomText.SetText(www.error);
        }
        else
        {

            // Or retrieve results as binary data
            var results = JsonHelper.FromJson<CalendarEventWithCategories>(www.downloadHandler.text);
            categories = results.categories;
            PopulateEvents(results);
            //randomText.SetText(results[1].color + " " + results[1].id);
        }
        LoadingPanel.SetActive(false);
    }

    private void ChangeCalendar(DateTime date)
    {
        canvas.GetChild(0).GetComponent<TextMeshProUGUI>().SetText(string.Format("{0} {1}", date.ToString("MMMM"), date.Year));

        var ca = CultureInfo.InvariantCulture.Calendar;
        var firstDayOfCurrentMonth = new DateTime(date.Year, date.Month, 1);
        firstDayCellIndex = (int)ca.GetDayOfWeek(firstDayOfCurrentMonth);

        var currentDateNotAltered = firstDayOfCurrentMonth;
        var currentDate = firstDayOfCurrentMonth;
        for (var i = firstDayCellIndex; i < 7; i++)
        {
            var c = grid.GetChild(i).GetComponent<CalendarCell>();
            c.ChangeDayText(currentDate.Day.ToString(), Color.black);
            currentDate = currentDate.AddDays(1);
        }

        var lastDayPreviousMonth = firstDayOfCurrentMonth.AddDays(-1);
        for (var i = firstDayCellIndex - 1; i >= 0; i--)
        {
            var c = grid.GetChild(i).GetComponent<CalendarCell>();
            c.ChangeDayText(lastDayPreviousMonth.Day.ToString(), Color.gray);
            lastDayPreviousMonth = lastDayPreviousMonth.AddDays(-1);
        }
        firstDayOfCalendar = lastDayPreviousMonth;

        for (var i = 7; i <= 41; i++)
        {
            var c = grid.GetChild(i).GetComponent<CalendarCell>();
            if (currentDate.Month != currentDateNotAltered.Month)
                c.ChangeDayText(currentDate.Day.ToString(), Color.gray);
            else
                c.ChangeDayText(currentDate.Day.ToString(), Color.black);
            currentDate = currentDate.AddDays(1);
        }

        lastDayOfCalendar = currentDate;
    }

    public void NextMonth()
    {
        ChangeCalendarMonth(currentDate.AddMonths(1));
    }

    public void PrevMonth()
    {
        ChangeCalendarMonth(currentDate.AddMonths(-1));
    }

    private void ChangeCalendarMonth(DateTime date)
    {
        ResetCalendar();

        currentDate = date;
        ChangeCalendar(currentDate);

        LoadingPanel.SetActive(true);
        StartCoroutine(GetEventData());
    }

    private void PopulateEvents(CalendarEventWithCategories events)
    {
       
        foreach (var e in events.calendarEvents)
        {
            var start = DateTime.Parse(e.start);
            var end = DateTime.Parse(e.end);

            var startDayOffset = start - firstDayOfCalendar;
            int startIdx = startDayOffset.Days > 0 ? startDayOffset.Days - 1 : 0;
            var endDayOffset = lastDayOfCalendar - end;//possivel bug - end devia de remover horas
            int endIdx = endDayOffset.Days > 0 ? 43 - endDayOffset.Days : 42;

            ICalendarSubCell prevEvent = null;

            for (var i = startIdx; i < endIdx; i++)
            {
                prevEvent = grid.GetChild(i).GetComponent<CalendarCell>().SetNewEvent(e, prevEvent);
            }
        }
    }

    private IEnumerator RefreshCalendar()
    {
        ResetCalendar();

        yield return StartCoroutine(GetEventData());
    }

    private void ResetCalendar()
    {
        for (var i = 0; i < grid.childCount; i++)
            grid.GetChild(i).GetComponent<CalendarCell>().Clear();
    }

    public IEnumerator CreateTodo(string name, string startDate, string endDate, string category)
    {
        var fields = new Dictionary<string, string>();
        fields.Add("Title", name);
        fields.Add("Start", startDate);
        fields.Add("End", endDate??string.Empty);
        fields.Add("Category", category);

        //UnityWebRequest www = UnityWebRequest.Post("https://localhost:44300/CreateEvent", fields);
        UnityWebRequest www = UnityWebRequest.Post("https://personalappv220210815170156.azurewebsites.net/CreateEvent", fields);
        www.SetRequestHeader("_requestVerificationToken", GetCSRFToken());
        //UnityWebRequest www = UnityWebRequest.Post("https://localhost:44307/Test", fields);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("success");
        }

        yield return StartCoroutine(RefreshCalendar());
        LoadingPanel.SetActive(false);
        CloseCreateTodoPopup();
    }

    public IEnumerator RemoveEvent(CalendarEvent calendarEvent)
    {
        //UnityWebRequest www = UnityWebRequest.Delete("https://localhost:44300/DeleteEvent/" + calendarEvent.id);
        UnityWebRequest www = UnityWebRequest.Delete("https://personalappv220210815170156.azurewebsites.net/DeleteEvent/" + calendarEvent.id);
        www.SetRequestHeader("_requestVerificationToken", GetCSRFToken());
        //UnityWebRequest www = UnityWebRequest.Delete("https://localhost:44307/Test/" + calendarEvent.id);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("success");
        }

        yield return StartCoroutine(RefreshCalendar());
        LoadingPanel.SetActive(false);
        CloseCreateTodoPopup();
    }

    public void OpenCreateTodoPopup()
    {
        InsertEventPopup.SetActive(true);
        InsertEventPopup.GetComponent<PopupForm>().SetCategories(categories) ;
    }

    public void CloseCreateTodoPopup()
    {
        InsertEventPopup.SetActive(false);
        EventDetailPanel.SetActive(false);
        ListEventsPopup.SetActive(false);
    }
}
