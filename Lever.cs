using Fixed_text_Adventure;

namespace Fixed_text_Adventure
{
    class Lever : IRoomResident
    {
        public string Name { get; } = "Lever";
        public string Description { get; } = "A rusty lever attached to the wall.";
        public bool IsFlipped { get; private set; } = false;
        public event Action? OnFlip;

        public void Flip()
        {
            IsFlipped = !IsFlipped;
            Console.WriteLine(IsFlipped ? "You flipped the lever. A door unlocks!" : "You reset the lever.");
            OnFlip?.Invoke();
        }

        public void Inspect()
        {
            Console.WriteLine(Description);
        }
    }
}
