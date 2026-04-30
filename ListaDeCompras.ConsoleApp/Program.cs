using System.Text;
using ListaDeCompras.ConsoleApp.Shared;

namespace ListaDeCompras.ConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        string title = Utils.ColourStringHex("Lista de Compras", Colours.Title);
        string[] options = ["Categorias", "Produtos", "Lista de Compras", "Sair"];

        while (true)
            switch (Utils.Menu(title, options))
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    return;
            }
    }
}
