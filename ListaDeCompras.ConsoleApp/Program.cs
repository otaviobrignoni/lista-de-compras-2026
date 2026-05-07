using System.Text;
using ListaDeCompras.ConsoleApp.CategoryModule;
using ListaDeCompras.ConsoleApp.ProductModule;
using ListaDeCompras.ConsoleApp.Shared;
using ListaDeCompras.ConsoleApp.ShoppingListModule;

namespace ListaDeCompras.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string title = Utils.ColourStringHex("Lista de Compras", Colours.Title);
        string[] options = ["Categorias", "Produtos", "Lista de Compras", "Sair"];

        CategoryRepo categoryRepo = new();
        CategoryUI cUI = new(categoryRepo);
        ProductRepo productRepo = new();
        ProductUI pUI = new(productRepo, cUI);
        ShoppingListRepo shoppingListRepo = new();
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
