using ListaDeCompras.ConsoleApp.ProductModule;
using ListaDeCompras.ConsoleApp.Shared;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.ShoppingListModule;

public class ShoppingListUI : BaseUserInterface<ShoppingList>, IShoppingListUI
{
    IProductUI pUI;
    public ShoppingListUI(IShoppingListRepo shoppingListRepo, IProductUI productUI) : base(shoppingListRepo)
    {
        pUI = productUI;
    }
    public override void Menu()
    {
        string title = Utils.ColourStringHex("Gerenciar listas de compras", Colours.Title);
        string[] options = ["Cadastrar lista de compras", "Editar lista de compras", "Remover lista de compras", "Visualizar listas de compras", "Voltar"];
        while (true)
            switch (Utils.Menu(title, options))
            {
                case 0:
                    Add();
                    break;
                case 1:
                    Edit();
                    break;
                case 2:
                    Remove();
                    break;
                case 3:
                    View();
                    break;
                case 4:
                    return;
            }
    }

    public override void Add()
    {
        if (!pUI.RepoHasAny())
        {
            Utils.MsgBox("Aviso", "Para cadastar uma lista de compras, primeiro cadastre um produto.", type: MessageType.Warning);
            return;
        }
        string title = Utils.ColourStringHex("Cadastrar lista de compras", Colours.Title);
        Repository.Add(new ShoppingList(GetValidName(title)));
        Utils.MsgBox("Sucesso", "Lista de compras cadastrada com sucesso!", type: MessageType.Success);
    }

    public override void Edit()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Aviso", "Nenhuma lista cadastrada para editar.", type: MessageType.Warning);
            return;
        }
        string title = Utils.ColourStringHex("Editar produto", Colours.Title);
        string[] options = ["Nome", "Itens da lista", "Voltar"];
        ShoppingList shoppingList = Select("Selecionar lista de compras para editar");
        ShoppingList editedList = new(shoppingList);
        while (true)
            switch (Utils.Menu(title, options))
            {
                case 0:
                    editedList.Name = GetValidName(title, [shoppingList]);
                    break;
                case 1:
                    ListItemMenu(shoppingList);
                    break;
                case 2:
                    if (!editedList.Equals(shoppingList))
                    {
                        Utils.MsgBox("Sucesso", "Lista de compras editada com sucesso!", type: MessageType.Success);
                        shoppingList.UpdateEntity(editedList);
                    }
                    return;
            }

    }

    public void ListItemMenu(ShoppingList shoppingList)
    {
        string title = $"Itens da lista \"{shoppingList.Name}\"";
        string[] categories = ["Produto", "Categoria", "Quantidade", $"Preço"];
        HashSet<ListItem> ignoredItems = [];
        while (true)
        {
            List<string[]> listItems = [];
            foreach (ListItem li in shoppingList.ListItems)
                listItems.Add([li.Product.Name, Utils.ColourStringHex(li.Product.Category.Name, li.Product.Category.Colour), $"{li.Quantity}", $"{li.Product.Price:C2} por {li.Product.MeasurementUnit}"]);
            Utils.GenerateTable(title, categories, listItems.ToArray(), false);
            Console.WriteLine($"ESC voltar | ENTER adicionar item | ESPAÇO remover item | Preço total estimado: {shoppingList.TotalCost:C2}");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    ListItem newListItem = AddItem();
                    ListItem? exisingItem = shoppingList.ListItems.FirstOrDefault(li => li.Product == newListItem.Product);
                    if (exisingItem is not null)
                        exisingItem.Quantity += newListItem.Quantity;
                    else
                    {
                        shoppingList.AddItem(newListItem);
                        ignoredItems.Add(newListItem);
                    }
                    break;
                case ConsoleKey.Spacebar:
                    if (shoppingList.ListItems.Count == 0)
                    {
                        Utils.MsgBox("Aviso", "Não há itens na lista.");
                        break;
                    }
                    ListItem listItem = SelectItem();
                    string thisTitle = $"Remover itens (max{listItem.Quantity})";
                    listItem.Quantity -= Utils.GetValidDecimal(thisTitle, "Quantidade: ", minValue: 0m, maxValue: listItem.Quantity);
                    if (listItem.Quantity == 0)
                    {
                        shoppingList.RemoveItem(listItem);
                        ignoredItems.Remove(listItem);
                    }
                    break;
                case ConsoleKey.Escape:
                    return;
            }
        }
        ListItem AddItem()
        {
            return new ListItem
            {
                Product = pUI.Select(),
                Quantity = Utils.GetValidDecimal("Adicionar item", "Quantidade de produtos: ", minValue: 0.001m)
            };
        }
        ListItem SelectItem()
        {
            string removeItemTitle = Utils.ColourStringHex("Remover item", Colours.Title);
            var availableItems = shoppingList.ListItems.Except(ignoredItems).ToList();
            string[] options = availableItems.Select(li => $"{li.Product.Name}").ToArray();
            return availableItems[Utils.Menu(removeItemTitle, options)];
        }
    }

    public override void Remove()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Info", "Nenhuma lista cadastrada para remover.", type: MessageType.Info);
            return;
        }
        ShoppingList shoppingList = Select("Selecionar lista de compras para remover");
        if (shoppingList.IsOpen)
        {
            Utils.MsgBox("Aviso", "Não é possível remover uma lista de compras aberta.", type: MessageType.Warning);
            return;
        }
        if (Repository.Remove(shoppingList.Id)) Utils.MsgBox("Sucesso", "Lista de compras removida com sucesso!", type: MessageType.Success);
        else Utils.MsgBox("Erro", "Erro ao remover a lista de compras. Tente novamente.", type: MessageType.Error);
    }

    public override ShoppingList Select(string? title = null, List<ShoppingList>? ignoredLists = null)
    {
        title ??= "Selecionar lista de compras";
        title = Utils.ColourStringHex(title, Colours.Title);
        var availableLists = GetAvailable(ignoredLists);
        string[] options = availableLists.Select(sl => $"{sl.Name}, ID:{sl.Id}").ToArray();
        return availableLists[Utils.Menu(title, options)];
    }

    public override void View()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Info", "Nenhuma lista cadastrada.", type: MessageType.Info);
            return;
        }
        string title = Utils.ColourStringHex("Listas de compras", Colours.Title);
        var orderedLists = Repository.GetAll().OrderBy(sl => sl.CreatedDay).ThenBy(sl => sl.ListItems.Count);
        List<string[]> lists = [];
        foreach (ShoppingList sl in orderedLists)
            lists.Add([sl.Name, $"{sl.ListItems.Count}", $"{sl.TotalCost():C2}", $"{sl.CreatedDay:dd/MM/yyyy}", Utils.ColourStringHex(sl.StatusString, sl.StatusColour)]);
        Utils.GenerateTable(title, ShoppingList.Categories, lists.ToArray());
    }
    public string GetValidName(string title, List<ShoppingList>? ignoredLists = null)
    {
        while (true)
        {
            string name = Utils.GetValidString(title, "Nome da lista de compras: ", minLength: 3, maxLength: null);
            if (GetAvailable(ignoredLists).Any(l => l.Name == name))
                Utils.MsgBox("Aviso", "Já existe uma lista de compras com esse nome.", type: MessageType.Warning);
            else return name;
        }
    }
}
