using System.Text;
using System.Text.Json;
using ListaDeCompras.ConsoleApp.CategoryModule;
using ListaDeCompras.ConsoleApp.ProductModule;
using ListaDeCompras.ConsoleApp.Shared;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;
using ListaDeCompras.ConsoleApp.ShoppingListModule;

namespace ListaDeCompras.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string title = Utils.ColourStringHex("Lista de Compras", Colours.Title);
        string[] options = ["Categorias", "Produtos", "Lista de Compras", "Sair"];

        JsonContext context = new();
        try
        {
            context.Load();
        }
        catch (JsonException)
        {
            Utils.MsgBox("Erro", "Ocorreu um erro ao ler dados salvos:\0JSON em formato inválido", type: MessageType.Error);
            return;
        }

        CategoryFileRepo categoryRepo = new(context);
        CategoryUI cUI = new(categoryRepo);
        ProductFileRepo productRepo = new(context);
        ProductUI pUI = new(productRepo, cUI);
        ShoppingListFileRepo shoppingListRepo = new(context);
        ShoppingListUI sUI = new(shoppingListRepo, pUI);

        while (true)
            switch (Utils.Menu(title, options))
            {
                case 0:
                    cUI.Menu();
                    break;
                case 1:
                    pUI.Menu();
                    break;
                case 2:
                    sUI.Menu();
                    break;
                case 3:
                    return;
            }
    }
}
