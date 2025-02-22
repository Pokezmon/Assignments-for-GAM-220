using Fixed_text_Adventure;
using System;

namespace Fixed_text_Adventure
{
    class Program
    {
        static void Main()
        {
            Room glade = new("Shaded Glade", "A densely packed forest with dim lighting. For some reason there is a locked door here");
            Room cave = new("Dark Cave", "A cold, damp cave with echoes of dripping water.", true);
            glade.AddExit("north", cave);
            cave.AddExit("south", glade);

            Item key = new("Key", "An old rusty key.");
            cave.AddRoomResident(key);

            Lever lever = new();
            glade.AddRoomResident(lever);
            lever.OnFlip += () => cave.Unlock();

            Player player = new(glade);
            Console.WriteLine("Welcome to Fallout: NORAD.");
            player.CurrentRoom.Inspect();

            while (true)
            {
                Console.Write("> ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";
                string[] parts = input.Split(' ', 2);
                string command = parts[0];
                string argument = parts.Length > 1 ? parts[1] : "";

                switch (command)
                {
                    case "look": player.CurrentRoom.Inspect(); break;
                    case "take": player.TakeItem(argument); break;
                    case "drop": player.DropItem(argument); break;
                    case "inventory": player.ShowInventory(); break;
                    case "flip": if (lever != null) lever.Flip(); break;
                    case "north": case "south": player.Move(command); break;
                    case "quit": return;
                    default: Console.WriteLine("Unknown command."); break;
                }
            }
        }
    }
}