using ListaDeCompras.ConsoleApp.Shared;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.CategoryModule;

public class CategoryUI : BaseUserInterface<Category>, ICategoryUI
{
    public CategoryUI(ICategoryRepo categoryRepo) : base(categoryRepo) { }
    public override void Menu()
    {
        string title = Utils.ColourStringHex("Gerenciar categorias", Colours.Title);
        string[] options = ["Cadastrar categoria", "Editar categoria", "Remover categoria", "Visualizar categorias", "Voltar"];
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
        string title = Utils.ColourStringHex("Adicionar categoria", Colours.Title);
        Repository.Add(new Category(GetValidName(title), GetValidColour(title)));
        Utils.MsgBox("Sucesso", "Categoria cadastrada com sucesso!", type: MessageType.Success);
    }

    public override void Edit()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Aviso", "Nenhuma categoria cadastrada para editar.", type: MessageType.Warning);
            return;
        }
        string title = Utils.ColourStringHex("Editar categoria", Colours.Title);
        string[] options = ["Nome", "Cor", "Voltar"];

        Category category = Select("Selecionar categoria para editar");
        Category editedCategory = new(category);

        while (true)
            switch (Utils.Menu(title, options))
            {
                case 0:
                    editedCategory.Name = GetValidName(title, [category]);
                    break;
                case 1:
                    editedCategory.Colour = GetValidColour(title);
                    break;
                case 2:
                    if (!editedCategory.Equals(category))
                    {
                        Utils.MsgBox("Sucesso", "Categoria editada com sucesso!", type: MessageType.Success);
                        category.UpdateEntity(editedCategory);
                    }
                    return;
            }
    }

    public override void Remove()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Info", "Nenhuma categoria cadastrada para remover.", type: MessageType.Info);
            return;
        }
        Category category = Select("Selecionar categoria para remover");
        if (category.Products.Count > 0)
        {
            Utils.MsgBox("Aviso", "Não é possível remover uma categoria com produtos vinculados.", type: MessageType.Warning);
            return;
        }
        if (Repository.Remove(category.Id)) Utils.MsgBox("Sucesso", "Categoria removida com sucesso!", type: MessageType.Success);
        else Utils.MsgBox("Erro", "Erro ao remover a categoria. Tente novamente.", type: MessageType.Error);
    }

    public override Category Select(string? title = null, List<Category>? ignoredCategories = null)
    {
        title ??= "Selecionar categoria";
        title = Utils.ColourStringHex(title, Colours.Title);
        var availableCategories = GetAvailable(ignoredCategories);
        string[] options = availableCategories.Select(c => $"{Utils.ColourStringHex(c.Name, c.Colour)}, ID: {c.Id}").ToArray();
        return availableCategories[Utils.Menu(title, options)];
    }

    public override void View()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Info", "Nenhuma categoria cadastrada.", type: MessageType.Info);
            return;
        }
        string title = Utils.ColourStringHex("Categorias", Colours.Title);
        List<string[]> categories = [];
        foreach (Category c in Repository.GetAll())
            categories.Add([Utils.ColourStringHex(c.Name, c.Colour), c.Colour]);
        Utils.GenerateTable(title, Category.Categories, categories.ToArray());
    }
    public string GetValidName(string title, List<Category>? ignoredCategories = null)
    {
        while (true)
        {
            string name = Utils.GetValidString(title, "Nome da categoria: ", minLength: 1, maxLength: 50);
            if (GetAvailable(ignoredCategories).Any(c => c.Name == name))
                Utils.MsgBox("Aviso", "Já existe uma categoria com esse nome.", type: MessageType.Warning);
            else return name;
        }
    }
    public static string GetValidColour(string title)
    {
        return Utils.GetValidString(title, "Cor da categoria: ", pattern: @"^#[0-9A-Fa-f]{6}$");
    }
}
