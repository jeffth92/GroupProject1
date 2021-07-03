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
        public enum Item { Sword, TreasureKey, HallKey, Mirror }; //mirror vs Gorgon item get image via ascii
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
            Console.Clear(); //if we make a title card, put it in this bit
            Console.WriteLine("You have snuck into the dungeon in search of treasure\n" +
                               "and have found The Dungeon's wealthy Treasury at last!\n" +
                               "The chest is far too heavy to be moved, it seems you'll\n" +
                               "need the right key in hand before you leave with the riches.\n" +
                                "------------------------------------------------------\n" +
                                "Press any key to start");
            Console.ReadLine();
            bool living = true;
            while (living)
            {
                
                Console.Clear();
                Console.WriteLine(currentRoom.Splash);
                foreach (Item item in currentRoom.Items)
                {
                    Console.WriteLine($"there is a {item} in this room.");
                }
                Console.ReadLine();
                string command = Console.ReadLine().ToLower();
                bool foundExit = false;
                if (command.StartsWith("go ") || command.StartsWith("move to ") || command.StartsWith("go to ") || command.StartsWith("enter ")) //go move enter: nsew considered at later time
                {
                    foreach (string exit in currentRoom.Exits) //room connection check
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
                    foreach (Item item in currentRoom.Items)
                    {
                        if (!foundItem && command.Contains(item.ToString().ToLower()))
                        {
                            Console.WriteLine($"You got {item}"
                                + " try using it!"
                                + " Press any key to continue..");
                            currentRoom.RemoveItem(item);
                            inventory.Add(item);
                            foundItem = true;
                            Console.ReadKey();
                            break;
                        }
                    }
                    if (!foundItem)
                    {
                        Console.WriteLine("You can't find one.");
                    }
                    foreach (Item item in currentRoom.Items)
                    {
                        Console.WriteLine($"You see one {item}.");
                    }
                }
                else if (command.StartsWith("use ") || command.StartsWith("activate ") || command.StartsWith("try ") ||
                      (inventory.Contains(Item.Sword) && command.StartsWith("swing sword")) ||
                      (inventory.Contains(Item.TreasureKey) && command.StartsWith("turn ")) ||
                      (inventory.Contains(Item.HallKey) && command.StartsWith("turn "))) //use activate ||swing sword execption? turn key (huge possibility of making a good text-parsing job.)
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
                    Console.WriteLine("Hint: use GO, GET, or USE. similar words might work."); //cant perform "exiting else" general case error message
                }
            }
        }  // Room geration here and non-random, hard-set outside of the while loop. next level from inside the chest?

        public static Room treasury = new Room(
            "You enter the treasury, as described here.\n" +
            "This chamber connects with the Cell and the Armory.",
            new List<string> { "cell", "armory" },
            new List<Item> { Item.Mirror },
            new List<Event> {
                new Event(
                "treasurekey",
                EventType.Use,
                new Result("goal", "You open the path downward into the Chest!")
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
           "Hall Text",
           new List<string> { "armory", "hiddenchamber" },
           new List<Item> { },
           new List<Event> {
                new Event(
                "sword",
                EventType.Use,
                new Result("Looking at your foe, You realize your mistake.")
                ),
                new Event(
                "mirror",
                EventType.Use,
                new Result("hiddenchamber", "You cause the monster to view itself, turning the Gorgon to stone!")
                ),
           }
       );

        public static Room hiddenchamber = new Room(
            "Secret room text",
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
            "You've beaten the demo.\n +" +
            "THANKS FOR PLAYING!",
            new List<string> { "treasury" },
            new List<Item> { },
            new List<Event> { }
        );
    }
}