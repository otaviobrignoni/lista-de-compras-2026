using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.ShoppingListModule;

public class ShoppingListFileRepo : BaseFileRepository<ShoppingList>, IShoppingListRepo
{
    public ShoppingListFileRepo(JsonContext context) : base(context) { }
    protected override Dictionary<Guid, ShoppingList> LoadEntities()
    {
        return Context.ShoppingLists;
    }
}
