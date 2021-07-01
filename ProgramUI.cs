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
        public enum Item { Sword, TreasureKey, CellKey, Mirror }; //mirror vs Gorgon item get image via ascii
        public List<Item> inventory = new List<Item>();

        readonly Dictionary<string, Room> Rooms = new Dictionary<string, Room>
        {
            {"Treasury", treasury },
           // {"Armory", armory },
           // {"Cell", cell },
           // {"Hall", hall },
            {"Goal", goal},
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
                Console.ReadLine();
                Console.Clear();
                Console.WriteLine(currentRoom.Splash);
                string command = Console.ReadLine().ToLower();
                bool foundExit = false;
                if (command.StartsWith("go ") || command.StartsWith("move to ") || command.StartsWith("go to ") || command.StartsWith("enter ")) //go move enter: nsew considered at later time
                {
                    foreach (string exit in currentRoom.Exits) //room connection check
                    {
                        if (command.Contains(exit) && //going to which room defined beneath, unneeded?!
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
                                + " Press any key to continue..");
                            currentRoom.RemoveItem(item);
                            inventory.Add(item);
                            foundItem = true;
                            Console.WriteLine();
                            Console.ReadKey();
                            break;
                        }
                    }
                    if (!foundItem)
                    {
                        Console.WriteLine("You can't find one.");
                    }
                }
                else if (command.StartsWith("use ") || command.StartsWith("activate ") || command.StartsWith("try ") ||
                      (inventory.Contains(Item.Sword) && command.StartsWith("swing sword")) ||
                      (inventory.Contains(Item.TreasureKey) && command.StartsWith("turn ")) ||
                      (inventory.Contains(Item.CellKey) && command.StartsWith("turn "))) //use activate ||swing sword execption? turn key (huge possibility of making a good text-parsing job.)
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
            "This chamber connects with the Cell and the Armory. You see the TreasureKey",
            new List<string> { "cell", "armory" },
            new List<Item> { Item.TreasureKey },
            new List<Event> {
                new Event(
                "use treasurekey",
                EventType.Use,
                new Result("goal", "You open the path Downward!")
                ),
                new Event(
                "get treasurekey",
                EventType.Get,
                new Result(Item.TreasureKey, "You found the Treasure Key! USE it in the Treasury!") //text doesn't seem to work
                )
            }
        );

        public static Room goal = new Room(
            "n\n\n\n\nYou've beaten the demo.\n",
            new List<string> { "treasury" },
            new List<Item> { },
            new List<Event> { }
        );
            
    }
}