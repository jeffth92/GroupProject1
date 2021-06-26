using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    public class ProgramUI
    {
        public enum Item { sword, TreasureKey };
        public List<Item> inventory = new List<Item>();

        Dictionary<string, Room> Rooms = new Dictionary<string, Room>
        {
            {"Treasury" treasury },
            {"Armory" armory },
            {"Cell" cell },
            {"Hall" hall },
        };

        public void Run()
        {
            Rooms currentRoom = treasury;
            Console.Clear(); //if we make a title card, put it in this bit
            Console.WriteLine("You have snuck into the dungeon in search of treasure.\n" +
                               "and have found The Treasury at last!");
        }
    }
}