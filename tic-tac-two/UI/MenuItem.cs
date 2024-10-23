namespace UI;
public class MenuItem
{
    public int  Number { get; set; }
    public string Name { get; set; }

    public Action? MenuItemAction { get; set; } // monel pole return value


    public MenuItem(int number, string name, Action? menuItemAction)
    {
        Number = number;
        Name = name;
        MenuItemAction = menuItemAction;
    }
}
