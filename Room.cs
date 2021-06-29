using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GroupProject.ProgramUI;

namespace GroupProject
{
    public class Room
    {
        public List<string> Exits { get; }
        public List<Item> Items { get; }
        public List<Event> Event { get; }

        public Room(string exits, List<Item> items, List<Events> event)
        {
            Exits = exits;
            Items = items;
            Events = events;
        }

        public void ResolveEvent(Event resolvedEvent)
        {
            if (Events.Contains(resolvedEvent))
            {
                Event
            }
        }
    }
}
