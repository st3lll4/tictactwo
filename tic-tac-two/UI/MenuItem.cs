namespace UI;
public record MenuItem
{
    public int Number { get; }
    public string Name { get; }

    public Func<string>? MenuItemAction { get; set; }


    public MenuItem(int number, string name)
    {
        Number = number;
        Name = name;
    }
}
