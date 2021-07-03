using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GroupProject.Event;

namespace GroupProject
{
    public class ProgramUI
    {
        public enum Item { Sword, TreasureKey, HallKey, Mirror }; //text
        public List<Item> inventory = new List<Item>();

        readonly Dictionary<string, Room> Rooms = new Dictionary<string, Room>
        {
            {"treasury", treasury },
            {"armory", armory },
            {"cell", cell },
            {"hall", hall },
            {"treasurechest", treasurechest},
            {"chest", treasurechest },
            {"treasure", treasurechest },
            {"hiddenchamber", hiddenchamber},
            {"hidden chamber", hiddenchamber},
            {"chamber", hiddenchamber}
        };

        public void Run()
        {
            Room currentRoom = treasury;
            Console.WriteLine("You have snuck into the dungeon in search of treasure\n" +
                               "and have found The Dungeon's wealthy Treasury at last!\n" +
                               "The chest is far too heavy to be moved, it seems you'll\n" +
                               "need the right key in hand before you leave with the riches.\n" +
                                "------------------------------------------------------\n" +
                                "Press any key to start");
            Console.ReadKey();
            Console.Clear();
            bool living = true;
            while (living)
            {
                Console.WriteLine(currentRoom.Splash);
                foreach (Item item in currentRoom.Items)
                {
                    Console.WriteLine($"there is a {item} in this room.");
                }
                string command = Console.ReadLine().ToLower();
                bool foundExit = false;
                if (command.StartsWith("go ") || command.StartsWith("move to ") || command.StartsWith("go to ") || command.StartsWith("enter "))
                {
                    foreach (string exit in currentRoom.Exits)
                    {
                        if (command.Contains(exit) &&
                        Rooms.ContainsKey(exit))
                        {
                            currentRoom = Rooms[exit];
                            foundExit = true;
                            break;
                        }
                    }
                    if (!foundExit)
                    {
                        Console.WriteLine("You cannot go there from here.");
                    }
                }
                else if (command.StartsWith("get ") || command.StartsWith("grab ") || command.StartsWith("take ") || command.StartsWith("pick up "))
                {
                    bool foundItem = false;
                    string eventMessage = "You can't find one.";
                    foreach (Item item in currentRoom.Items)
                    {
                        if (!foundItem && command.Contains(item.ToString().ToLower()))
                        {
                            inventory.Add(item);
                            currentRoom.RemoveItem(item);
                            foundItem = true;
                            foreach(Event roomEvent in currentRoom.Events)
                            {
                                if (roomEvent.EventResult.Type == Result.ResultType.GetItem)
                                {
                                    eventMessage = roomEvent.EventResult.ResultMessage;
                                }
                            }
                            break;
                        }
                    }
                    Console.WriteLine(eventMessage);
                }
                else if (command.StartsWith("use ") || command.StartsWith("activate ") || command.StartsWith("try ") ||
                      (inventory.Contains(Item.Sword) && command.StartsWith("swing sword")) ||
                      (inventory.Contains(Item.TreasureKey) && command.StartsWith("turn ")) ||
                      (inventory.Contains(Item.HallKey) && command.StartsWith("turn ")))
                {
                    string eventMessage = "You try, but nothing happens.";
                    foreach (Event roomEvent in currentRoom.Events)
                    {
                        if (!command.Contains(roomEvent.TriggerPhrase) || roomEvent.Type != EventType.Use)
                        {
                            continue;
                        }
                        if (roomEvent.EventResult.Type == Result.ResultType.NewExit)
                        {
                            currentRoom.Exits.Add(roomEvent.EventResult.ResultExit);
                            eventMessage = roomEvent.EventResult.ResultMessage;
                        }
                        else if (roomEvent.EventResult.Type == Result.ResultType.GetItem)
                        {
                            inventory.Add(roomEvent.EventResult.ResultItem);
                            eventMessage = roomEvent.EventResult.ResultMessage;
                        }
                        else if (roomEvent.EventResult.Type == Result.ResultType.MessageOnly)
                        {
                            eventMessage = roomEvent.EventResult.ResultMessage;
                        }
                    }
                    Console.WriteLine(eventMessage);
                }
                else
                {
                    Console.WriteLine("Hint: use GO, GET, or USE. similar words might work.");
                }
                //DEATH CHECKS
                if (currentRoom == hall && command.Contains("sword"))   //DEATH CHECK Medusa
                {
                    living = false;
                    Console.Clear();
                    Console.WriteLine("You've Perished, Play again? Y/N");
                    string PlayAgain = Console.ReadLine();
                    if (PlayAgain.ToLower() == "y")
                    {
                        Run();
                    }
                }
                //cleaning screen
                Console.WriteLine("---------------------------------------------\n" + "Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        public static Room treasury = new Room(
            "You enter the treasury, as described here.\n" +
            "This chamber connects with the Cell and the Armory.",
            new List<string> { "cell", "armory" },
            new List<Item> { Item.Mirror },
            new List<Event> {
                new Event(
                "treasurekey",
                EventType.Use,
                new Result("treasurechest", "You open the path downward into the Chest!")
                ),
                new Event(
                "mirror",
                EventType.Get,
                new Result(Item.Mirror, "You found a Mirror, Useful for looking at your reflection..")
                ),
            }
        );
        public static Room armory = new Room(
            "Armory Text",
            new List<string> { "treasury", "hall" },
            new List<Item> { Item.Sword },
            new List<Event> {
                new Event(
                "sword",
                EventType.Get,
                new Result(Item.Sword, "You found the Sword! Use it in a fight!")
                ),
            }
        );

        public static Room cell = new Room(
            "Cell Text",
            new List<string> { "treasury" },
            new List<Item> { Item.HallKey },
            new List<Event> {  
                new Event(
                "hallkey",
                EventType.Get,
                new Result(Item.HallKey, "You found the Hallkey! Where is the Hall?")
                ),
            }
        );

        public static Room hall = new Room(
           "Hall Text: You are aware of a Gorgon somewhere in this room.",
           new List<string> { "armory", "hiddenchamber" },
           new List<Item> { },
           new List<Event> {
                new Event(
                "sword",
                EventType.Use,
                new Result("Looking at your foe, You realize your mistake! You've been petrified.")
                ),
                new Event(
                "mirror",
                EventType.Use,
                new Result("hiddenchamber", "You cause the monster to view itself, turning the Gorgon to stone!")
                ),
           }
       );

        public static Room hiddenchamber = new Room(
            "THis small room is shaped so you cannot go back. You see that a tunnel might lead to the Treasury!",
            new List<string> { "treasury" },
            new List<Item> { Item.TreasureKey },
            new List<Event> { 
            new Event(
                "treasurekey",
                EventType.Get,
                new Result(Item.TreasureKey, "You found the Treasure Key! USE it in the Treasury!")
                ),
            }
        );

        public static Room treasurechest = new Room(
            "You've beaten the demo.\n" +
            "THANKS FOR PLAYING!",
            new List<string> { "treasury" },
            new List<Item> { },
            new List<Event> { }
        );
    }
}