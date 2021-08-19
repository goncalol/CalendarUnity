using UnityEngine;

public class Button : MonoBehaviour
{
    public bool NextMonth;
    public Calendar Calendar;

    void OnMouseDown()
    {
        if (NextMonth)
            Calendar.NextMonth();
        else
            Calendar.PrevMonth();
    }
}
