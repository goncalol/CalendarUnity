using Assets.Scripts.Messages;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts
{
    public class CalendarSubCell : MonoBehaviour, ICalendarSubCell, IPointerClickHandler
    {
        public CalendarSubCell AboveSubCell;
        public CalendarSubCell BellowSubCell;
        public int Position;

        private ICalendarSubCell next;
        private ICalendarSubCell prev;
        private TextMeshProUGUI tmp;
        private CalendarEvent calendarEvent;
        private List<CalendarSubCellSpecial> accumulattedTexts;
        private GameObject eventDetailPanel;
        private GameObject listEventsPopup;
        public GameObject listEventItem;

        internal bool IsSingle() => next == null && prev == null;

        private void Awake()
        {
            var calendar = GameObject.FindGameObjectWithTag("Calendar").GetComponent<Calendar>();
            eventDetailPanel = calendar.EventDetailPanel;
            listEventsPopup = calendar.ListEventsPopup;
            listEventItem = calendar.ListEventItem;
            tmp = gameObject.GetComponent<TextMeshProUGUI>();
            tmp.SetText(string.Empty);
            accumulattedTexts = new List<CalendarSubCellSpecial>();
        }

        internal void SetNewEvent(ICalendarSubCell prev, CalendarEvent calendarEvent)
        {
            prev?.SetNext(this);
            this.prev = prev;
            this.calendarEvent = calendarEvent;
            SetText(calendarEvent);
        }
        
        public void SetNext(ICalendarSubCell calendarSubCell) => next = calendarSubCell;

        internal void SetText(CalendarEvent calendarEvent)
        {
            var date= DateTime.Parse(calendarEvent.start);
            var dateString = date.ToString("HH:mm");
            if (dateString == "00:00" || prev!=null)
                tmp.SetText(string.Format("<sprite index=0 color={0}> {1}", calendarEvent.color, calendarEvent.title));
            else
                tmp.SetText(string.Format("<sprite index=0 color={0}> {1} {2}", calendarEvent.color, dateString, calendarEvent.title));
        }

        internal bool IsEmpty() => tmp.text == string.Empty;

        public string GetText() => tmp?.text ?? string.Empty;

        internal CalendarSubCellSpecial SetMore(ICalendarSubCell prev, CalendarEvent calendarEvent)
        {
            if (accumulattedTexts.Count == 0)
            {
                accumulattedTexts.Add(new CalendarSubCellSpecial(this.calendarEvent, next,this.prev));
                tmp.SetText("<b>see more</b>");
            }
            var newOne = new CalendarSubCellSpecial(calendarEvent, null, prev);
            accumulattedTexts.Add(newOne);
            this.next = null;
            this.calendarEvent = null;
            this.prev = null;
            return newOne;
        }

        internal void Clear()
        {
            next = null;
            prev = null;
            calendarEvent = null;
            tmp.SetText(string.Empty);
            accumulattedTexts = new List<CalendarSubCellSpecial>();
        }

        internal string Click()
        {
            var s = "";
            foreach (var ee in accumulattedTexts)
            {
                s += ee + "_";
            }
            return s;
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (accumulattedTexts.Count == 0)
            {
                eventDetailPanel.SetActive(true);
                eventDetailPanel.GetComponent<PopupEventDetail>().SetChosenEvent(calendarEvent);
            }
            else
            {
                listEventsPopup.SetActive(true);

                var listEventContent = listEventsPopup.transform.GetChild(3).GetChild(0).GetChild(0);
                var childs = listEventContent.childCount;
                for (var i = childs - 1; i >= 0; i--)
                {
                    Destroy(listEventContent.GetChild(i).gameObject);
                }

                foreach (var accumulattedText in accumulattedTexts)
                {
                    var go = Instantiate(listEventItem);
                    go.GetComponent<EventListItem>().SetItem(accumulattedText.GetEvent());
                    go.transform.SetParent(listEventContent);
                }
            }
        }

        //internal void PushDown()
        //{
        //    if (BellowSubCell != null)
        //    {
        //        if (!BellowSubCell.gameObject.activeInHierarchy)
        //        {
        //            BellowSubCell.gameObject.SetActive(true);
        //            next?.PushDown();
        //            BellowSubCell.CopyFrom(this);
        //            Clear();
        //        }
        //        else
        //        {
        //            BellowSubCell.PushDown();
        //            next?.PushDown();
        //            CopyFrom(AboveSubCell);
        //            AboveSubCell?.Clear();
        //        }
        //    }
        //    else
        //    {
        //        if (accumulattedTexts.Count == 0)
        //        {
        //            accumulattedTexts.Add(GetText());
        //            tmp.SetText("<b>see more</b>");
        //        }
        //        var aboveTxt = AboveSubCell?.GetText();
        //        if (!string.IsNullOrWhiteSpace(aboveTxt))
        //            accumulattedTexts.Add(aboveTxt);

        //        next?.PushDown();
        //        next = null;
        //    }

        //}

        //internal bool IsEmpty() => tmp.text == string.Empty;

        //internal void SetNewEvent(CalendarSubCell prev, string title)
        //{
        //    if (Position != 4)
        //    {
        //        prev?.SetNext(this);
        //        this.prev = prev;
        //        if (tmp.text != string.Empty)
        //            PushDown();

        //        SetText(title);
        //    }
        //    else SetMore(title);
        //}

        //private void SetNext(CalendarSubCell calendarSubCell) => next = calendarSubCell;

        //internal int GetPosition() => Position;

        //internal void SetMore(string title)
        //{
        //    if (accumulattedTexts.Count == 0)
        //    {
        //        accumulattedTexts.Add(GetText());
        //        tmp.SetText("<b>see more</b>");
        //    }
        //    accumulattedTexts.Add(title);
        //    next?.PushDown();
        //    next = null;
        //    prev = null;
        //}


        //internal void SetText(string title) => tmp.SetText(title);

        //private void Clear()
        //{
        //    if (gameObject.activeInHierarchy)
        //    {
        //        next = null;
        //        prev = null;
        //        tmp.SetText(string.Empty);
        //    }
        //}

        //private void CopyFrom(CalendarSubCell calendarSubCell)
        //{
        //    if (calendarSubCell.gameObject.activeInHierarchy)
        //    {
        //        next = calendarSubCell?.next;
        //        prev = calendarSubCell?.prev;
        //        tmp.SetText(calendarSubCell?.GetText() ?? string.Empty);
        //    }
        //}

        //public string GetText() => tmp?.text ?? string.Empty;


    }
}
