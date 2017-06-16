using RaceDay.Model;
using System.Collections.ObjectModel;

namespace RaceDay.Helpers
{
    public static class ObservableEventCollection
    {
        public static void AddEvent(this ObservableCollection<Event> collection, Event newEvent )
        {
            int index = 0;
            foreach (var _event in collection)
            {
                if (_event.Date > newEvent.Date)
                    break;
                index++;
            }
            if (index < collection.Count)
                collection.Insert(index, newEvent);
            else
                collection.Add(newEvent);
        }

        public static void DeleteEvent(this ObservableCollection<Event> collection, int eventId)
        {
            int index = 0;
            foreach (var _event in collection)
            {
                if (_event.EventId == eventId)
                    break;
                index++;
            }

            if (index < collection.Count)
                collection.RemoveAt(index);
        }
    }
}
