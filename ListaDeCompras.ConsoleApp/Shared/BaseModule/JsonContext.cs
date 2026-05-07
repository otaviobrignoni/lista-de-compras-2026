using System.Text.Json;
using System.Text.Json.Serialization;
using ListaDeCompras.ConsoleApp.CategoryModule;
using ListaDeCompras.ConsoleApp.ProductModule;
using ListaDeCompras.ConsoleApp.ShoppingListModule;

namespace ListaDeCompras.ConsoleApp.Shared.BaseModule;

public class JsonContext
{
    public Dictionary<Guid, Category> Categories { get; set; } = [];
    public Dictionary<Guid, Product> Products { get; set; } = [];
    public Dictionary<Guid, ShoppingList> ShoppingLists { get; set; } = [];
    private readonly string FilePath;
    private readonly JsonSerializerOptions options;
    public JsonContext()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string folderPath = Path.Combine(appDataPath, "ListaDeCompras");

        Directory.CreateDirectory(folderPath);
        FilePath = Path.Combine(folderPath, "savedData.json");
        options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            IncludeFields = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };
    }
    public void Save()
    {
        string jsonString = JsonSerializer.Serialize(this, options);
        File.WriteAllText(FilePath, jsonString);
    }
    public void Load()
    {
        if (!File.Exists(FilePath)) return;

        string jsonString = File.ReadAllText(FilePath);

        JsonContext? context = JsonSerializer.Deserialize<JsonContext>(jsonString, options);

        if (context is null) return;

        Categories = context.Categories;
        Products = context.Products;
        ShoppingLists = context.ShoppingLists;
    }
}
