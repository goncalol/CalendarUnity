using System;

namespace Assets.Scripts.Messages
{
    [Serializable]
    public class CalendarEvent
    {
        public int id;
        public string color;
        public string title;
        public string start;
        public string end;
    }
}
