using Fixed_text_Adventure;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Fixed_text_Adventure
{
    class Player
    {
        public Room CurrentRoom { get; private set; }
        private List<Item> Inventory { get; } = new();

        public Player(Room startRoom)
        {
            CurrentRoom = startRoom;
        }

        public void Move(string direction)
        {
            Room? nextRoom = CurrentRoom.GetExit(direction);
            if (nextRoom != null)
            {
                CurrentRoom = nextRoom;
                Console.WriteLine($"You moved {direction}.");
                CurrentRoom.Inspect();
            }
            else
            {
                Console.WriteLine("You can't go that way.");
            }
        }

        public void TakeItem(string itemName)
        {
            Item? item = CurrentRoom.FindResident(itemName) as Item;
            if (item != null)
            {
                Inventory.Add(item);
                CurrentRoom.RemoveRoomResident(item);
                Console.WriteLine($"You took the {item.Name}.");
            }
            else
            {
                Console.WriteLine("That item is not here.");
            }
        }

        public void DropItem(string itemName)
        {
            Item? item = Inventory.Find(i => i.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                Inventory.Remove(item);
                CurrentRoom.AddRoomResident(item);
                Console.WriteLine($"You dropped the {item.Name}.");
            }
            else
            {
                Console.WriteLine("You don't have that item.");
            }
        }

        public void ShowInventory()
        {
            Console.WriteLine("Your inventory:");
            if (Inventory.Count == 0)
            {
                Console.WriteLine(" - Empty");
            }
            else
            {
                foreach (var item in Inventory)
                {
                    Console.WriteLine(" - " + item.Name);
                }
            }
        }
    }
}