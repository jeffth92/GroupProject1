using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupProject
{
    public class ProgramUI
    {
        public enum Item { Sword, TreasureKey, CellKey, Mirror }; //mirror vs Gorgon item get image via ascii
        public List<Item> inventory = new List<Item>();

      //  Dictionary<string, Room> Rooms = new Dictionary<string, Room>
      //  {
      //      {"Treasury", treasury },
      //      {"Armory", armory },
      //      {"Cell", cell },
      //      {"Hall", hall },
      //  };

        public void Run()
        {
            // Room currentRoom = treasury;
            Console.Clear(); //if we make a title card, put it in this bit
            Console.WriteLine("You have snuck into the dungeon in search of treasure.\n" +
                               "and have found The Dungeon's wealthy Treasury at last!\n" +
                               "The chest is far too heavy to be moved, it seems you'll\n" +
                               "need the right key in hand before you leave with the riches.\n" +
                                "------------------------------------------------------");
            Console.ReadKey();
            bool living = true;
            while (living)
            {
                Console.Clear();
                Console.WriteLine($"You're in the {currentRoom}");
                //go exit move leave: nsew considered at later time

                //going to which room defined beneath ^

                //get take grab pick_up

                //use activate ||swing sword execption? turn key (huge possibility of making a good text-parsing job.) 

                //cant perform "exiting else" general case error message
            }
        }  // Room geration here and non-random, hard-set outside of the while loop. next level from inside the chest?
    }
}