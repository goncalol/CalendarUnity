using System;

namespace Assets.Scripts.Messages
{
    [Serializable]
    public class CalendarEventWithCategories
    {
        public CalendarEvent[] calendarEvents;
        public Category[] categories;
    }
}
