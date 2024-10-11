namespace UI;
public class MenuItem
{
    public int Number { get; }
    public string Name { get; }
    public Action Action { get; }

    public MenuItem(int number, string name, Action action)
    {
        Number = number;
        Name = name;
        Action = action;
    }
}
