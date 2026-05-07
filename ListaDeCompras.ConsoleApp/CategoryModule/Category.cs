using ListaDeCompras.ConsoleApp.ProductModule;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.CategoryModule;

public class Category : BaseEntity<Category>
{
    public string Name { get; set; } = string.Empty;
    public string Colour { get; set; } = string.Empty;
    public HashSet<Product> Products = [];
    public static readonly string[] Categories = ["Nome", "Código da cor"];

    public Category() { }

    public Category(string name, string colour)
    {
        Name = name;
        Colour = colour;
    }
    public Category(Category c) : this(c.Name, c.Colour) { }
    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
    public bool RemoveProduct(Product product)
    {
        return Products.Remove(product);
    }

    public override bool Equals(Category category)
    {
        if (category.Name != Name || category.Colour != Colour)
            return false;
        return true;
    }
    public override void UpdateEntity(Category updatedCategory)
    {
        Name = updatedCategory.Name;
        Colour = updatedCategory.Colour;
    }
}
