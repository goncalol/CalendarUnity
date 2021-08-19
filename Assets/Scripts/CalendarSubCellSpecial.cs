using Assets.Scripts.Messages;

namespace Assets.Scripts
{
    public class CalendarSubCellSpecial : ICalendarSubCell
    {
        private ICalendarSubCell next;
        private ICalendarSubCell prev;
        private CalendarEvent calendarEvent;

        public CalendarSubCellSpecial(CalendarEvent calendarEvent, ICalendarSubCell next, ICalendarSubCell prev)
        {
            this.calendarEvent = calendarEvent;
            this.next = next;
            this.prev = prev;
        }

        public void SetNext(ICalendarSubCell subCell) => next = subCell;

        public CalendarEvent GetEvent() => calendarEvent;
    }
}
