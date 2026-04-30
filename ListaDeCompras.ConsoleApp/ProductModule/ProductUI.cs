using ListaDeCompras.ConsoleApp.CategoryModule;
using ListaDeCompras.ConsoleApp.Shared;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.ProductModule;

public class ProductUI : BaseUserInterface<Product>, IProductUI
{
    private readonly ICategoryUI cUI;
    public ProductUI(IProductRepo productRepo, ICategoryUI categoryUI) : base(productRepo)
    {
        cUI = categoryUI;
    }
    public override void Menu()
    {
        string title = Utils.ColourStringHex("Gerenciar produtos", Colours.Title);
        string[] options = ["Cadastrar produto", "Editar produto", "Remover produto", "Visualizar produtos", "Voltar"];
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
        if (!cUI.RepoHasAny())
        {
            Utils.MsgBox("Aviso", "Para cadastar um produto, primeiro cadastre uma categoria.", type: MessageType.Warning);
            return;
        }
        string title = Utils.ColourStringHex("Cadastrar produto", Colours.Title);
        var (name, category) = GetValidNameAndCategory(title);
        Product product = new(name, category, Utils.PromptBox(title, "Unidade de medida: "), GetValidPrice(title));
        product.Category.AddProduct(product);
        Repository.Add(product);
        Utils.MsgBox("Sucesso", "Produto cadastrado com sucesso!", type: MessageType.Success);
    }

    public override void Edit()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Aviso", "Nenhuma produto cadastrado para editar.", type: MessageType.Warning);
            return;
        }
        string title = Utils.ColourStringHex("Editar produto", Colours.Title);
        string[] options = ["Nome e Categoria", "Unidade de medida", "Preço", "Voltar"];

        Product product = Select("Selecionar produto para editar");
        Product editedProduct = new(product);

        while (true)
            switch (Utils.Menu(title, options))
            {
                case 0:
                    var (name, category) = GetValidNameAndCategory(title, [product]);
                    editedProduct.Name = name;
                    editedProduct.Category = category;
                    break;
                case 1:
                    editedProduct.MeasurementUnit = Utils.PromptBox(title, "Unidade de medida: ");
                    break;
                case 2:
                    editedProduct.Price = GetValidPrice(title);
                    break;
                case 3:
                    if (!editedProduct.Equals(product))
                    {
                        Utils.MsgBox("Sucesso", "Produto editado com sucesso!", type: MessageType.Success);
                        product.UpdateEntity(editedProduct);
                    }
                    return;
            }
    }

    public override void Remove()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Info", "Nenhum produto cadastrado para remover.", type: MessageType.Info);
            return;
        }
        Product product = Select("Selecionar produto para remover");
        if (Repository.Remove(product.Id))
        {
            Utils.MsgBox("Sucesso", "Produto removido com sucesso!", type: MessageType.Success);
            product.Category.RemoveProduct(product);
        }
        else Utils.MsgBox("Erro", "Erro ao remover o produto. Tente novamente.", type: MessageType.Error);
    }

    public override Product Select(string? title = null, List<Product>? ignoredProducts = null)
    {
        title ??= "Selecionar produto";
        title = Utils.ColourStringHex(title, Colours.Title);
        var availableProducts = GetAvailable(ignoredProducts);
        string[] options = availableProducts.Select(p => $"{Utils.ColourStringHex(p.Category.Name, p.Category.Colour)}: {p.Name}, ID: {p.Id}").ToArray();
        return availableProducts[Utils.Menu(title, options)];
    }

    public override void View()
    {
        if (!RepoHasAny())
        {
            Utils.MsgBox("Info", "Nenhum produto cadastrado.", type: MessageType.Info);
            return;
        }
        string title = Utils.ColourStringHex("Produtos", Colours.Title);
        List<string[]> products = [];
        foreach (Product p in Repository.GetAll())
            products.Add([p.Name, Utils.ColourStringHex(p.Category.Name, p.Category.Colour), p.MeasurementUnit, $"{p.Price:C2} por {p.MeasurementUnit}"]);
        Utils.GenerateTable(title, Product.Categories, products.ToArray());
    }

    public (string, Category) GetValidNameAndCategory(string title, List<Product>? ignoredProducts = null)
    {
        ignoredProducts ??= [];
        while (true)
        {
            Category category = cUI.Select("Selecionar categoria do produto");
            string name = Utils.GetValidString(title, "Nome do produto: ", minLength: 2, maxLength: null);
            if (category.Products.Except(ignoredProducts).Any(p => p.Name == name))
                Utils.MsgBox("Aviso", "Já existe um produto com esse nome nessa categoria.", type: MessageType.Warning);
            else return (name, category);
        }
    }
    public static decimal GetValidPrice(string title)
    {

        while (true)
            if (decimal.TryParse(Utils.PromptBox(title, "Preço do produto: "), out decimal value))
                if (value <= 0)
                    Utils.MsgBox("Aviso", $"O preço deve ser maior que zero", type: MessageType.Warning);
                else
                    return value;
            else
                Utils.MsgBox("Aviso", "Insira um valor númerico válido", type: MessageType.Warning);
    }
}
