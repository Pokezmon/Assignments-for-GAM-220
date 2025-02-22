using Fixed_text_Adventure;

namespace Fixed_text_Adventure
{
    class Item : IRoomResident
    {
        public string Name { get; private set; }
        public string Description { get; private set; }

        public Item(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void Inspect()
        {
            Console.WriteLine(Description);
        }
    }
}