using Fixed_text_Adventure;
using System;
using System.Collections.Generic;

namespace Fixed_text_Adventure
{
    class Room : IInspectable
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        private Dictionary<string, Room> Exits { get; } = new();
        private List<IRoomResident> Residents { get; } = new();
        public bool IsLocked { get; private set; } = false;

        public event Action? OnUnlock;

        public Room(string name, string description, bool isLocked = false)
        {
            Name = name;
            Description = description;
            IsLocked = isLocked;
        }

        public void AddExit(string direction, Room room)
        {
            Exits[direction] = room;
        }

        public Room? GetExit(string direction)
        {
            return Exits.TryGetValue(direction, out Room? room) ? (room.IsLocked ? null : room) : null;
        }

        public void Unlock()
        {
            IsLocked = false;
            OnUnlock?.Invoke();
        }

        public void AddRoomResident(IRoomResident resident)
        {
            Residents.Add(resident);
        }

        public void RemoveRoomResident(IRoomResident resident)
        {
            Residents.Remove(resident);
        }

        public void Inspect()
        {
            Console.WriteLine($"{Name}: {Description}");
            if (Residents.Count > 0)
            {
                Console.WriteLine("You see:");
                foreach (var resident in Residents)
                {
                    Console.WriteLine(" - " + resident.Name);
                }
            }
            Console.WriteLine("Exits:");
            foreach (var exit in Exits.Keys)
            {
                Console.WriteLine(" - " + exit);
            }
        }

        public IRoomResident? FindResident(string name)
        {
            return Residents.Find(r => r.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
    }
}