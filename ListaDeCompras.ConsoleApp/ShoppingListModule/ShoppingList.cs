using ListaDeCompras.ConsoleApp.Shared;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.ShoppingListModule;

public class ShoppingList : BaseEntity<ShoppingList>
{
    public string Name { get; internal set; } = string.Empty;
    public DateOnly CreatedDay;
    public ShoppingListStatus CurrentStatus => ListItems.Count > 0 ? ShoppingListStatus.Open : ShoppingListStatus.Done;
    public string StatusString => CurrentStatus switch
    {
        ShoppingListStatus.Open => "Aberta",
        ShoppingListStatus.Done => "Concluída",
        _ => string.Empty
    };
    public string StatusColour => CurrentStatus switch
    {
        ShoppingListStatus.Open => Colours.LightBlue,
        ShoppingListStatus.Done => Colours.LightGreen,
        _ => Colours.White
    };
    public bool IsOpen => CurrentStatus == ShoppingListStatus.Open;
    public HashSet<ListItem> ListItems = [];
    public static readonly string[] Categories = ["Nome", "N° de itens", "Valor estimado total", "Data de criação", "Status"];
    public ShoppingList(string name, DateOnly? createdDay = null)
    {
        createdDay ??= DateOnly.FromDateTime(DateTime.Now);
        Name = name;
        CreatedDay = (DateOnly)createdDay;
    }
    public ShoppingList(ShoppingList shoppingList) : this(shoppingList.Name, shoppingList.CreatedDay) { }

    public void AddItem(ListItem listItem)
    {
        ListItems.Add(listItem);
    }
    public void RemoveItem(ListItem listItem)
    {
        ListItems.Remove(listItem);
    }
    public decimal TotalCost()
    {
        decimal price = 0;
        foreach (ListItem li in ListItems)
        {
            price += li.Product.Price * li.Quantity;
        }
        return price;
    }
    public override bool Equals(ShoppingList shoppingList)
    {
        if (Name != shoppingList.Name || CreatedDay != shoppingList.CreatedDay)
            return false;
        return true;
    }
    public override void UpdateEntity(ShoppingList updatedShoppingList)
    {
        Name = updatedShoppingList.Name;
    }
}
