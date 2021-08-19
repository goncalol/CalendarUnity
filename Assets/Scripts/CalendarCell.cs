using Assets.Scripts;
using Assets.Scripts.Messages;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CalendarCell : MonoBehaviour, IPointerClickHandler
{
    private TextMeshProUGUI dayText;

    private void Awake()
    {
        dayText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    internal void ChangeDayText(string t, Color c)
    {
        dayText.SetText(t);
        dayText.color = c;
    }

    internal ICalendarSubCell SetNewEvent(CalendarEvent calendarEvent, ICalendarSubCell prevEvent)
    {
        var slot = GetAvailableSlot();

        if (slot != null)
        {
            slot.SetNewEvent(prevEvent, calendarEvent);
            return slot;
        }
        else
        {
            var subCell = transform.GetChild(4).gameObject.GetComponent<CalendarSubCell>();

            return subCell.SetMore(prevEvent, calendarEvent);
        }
    }

    internal CalendarSubCell GetAvailableSlot()
    {
        for (int i = 1; i <= 4; i++)
        {
            var subCellGO = transform.GetChild(i).gameObject;
            var SubCell = subCellGO.GetComponent<CalendarSubCell>();
            if (!subCellGO.activeInHierarchy || SubCell.IsEmpty())
            {
                subCellGO.SetActive(true);
                return SubCell;
            }
        }
        return null;
    }

    internal void Clear()
    {
        for (int i = 1; i <= 4; i++)
        {
            var subCellGO = transform.GetChild(i).gameObject;
            var SubCell = subCellGO.GetComponent<CalendarSubCell>();
            if (subCellGO.activeInHierarchy)
            {
                SubCell.Clear();
                subCellGO.SetActive(false);
            }
        }
    }


    //internal CalendarSubCell SetNewEvent(string title, CalendarSubCell prevEvent)
    //{
    //    if (prevEvent == null)
    //    {
    //        var slot = GetAvailableSlot();

    //        if (slot != null)
    //        {
    //            slot.SetText(title);
    //            return slot;
    //        }
    //        else
    //        {
    //            var subCell = transform.GetChild(4).gameObject.GetComponent<CalendarSubCell>();
    //            subCell.SetMore(title);
    //            return subCell;
    //        }
    //    }
    //    else
    //    {
    //        var slotGO = transform.GetChild(prevEvent.GetPosition()).gameObject;
    //        var slot = slotGO.GetComponent<CalendarSubCell>();
    //        if (!slotGO.activeInHierarchy)
    //            slotGO.SetActive(true);

    //        slot.SetNewEvent(prevEvent, title);
    //        return slot;
    //    }
    //}

    //internal CalendarSubCell GetAvailableSlot()
    //{
    //    for (int i = 1; i <= 4; i++)
    //    {
    //        var subCellGO = transform.GetChild(i).gameObject;
    //        var SubCell = subCellGO.GetComponent<CalendarSubCell>();
    //        if (!subCellGO.activeInHierarchy || SubCell.IsEmpty())
    //        {
    //            subCellGO.SetActive(true);
    //            return SubCell;
    //        }
    //        else if (SubCell.IsSingle() && i<4)
    //        {
    //            SubCell.PushDown();
    //            return SubCell;
    //        }
    //    }
    //    return null;
    //}


    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(transform.GetChild(4).gameObject.GetComponent<CalendarSubCell>().Click());
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    text.fontSize = 1.5f;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    text.fontSize = 0.5f;
    //}
}
