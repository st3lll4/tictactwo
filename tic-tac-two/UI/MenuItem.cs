namespace UI
{
    public class MenuItem
    {
        public int Number { get; private set; }
        public string Name { get; private set; }
        public Action Action { get; private set; }

        public MenuItem(int number, string name, Action action)
        {
            Number = number;
            Name = name;
            Action = action;
        }
    }
}