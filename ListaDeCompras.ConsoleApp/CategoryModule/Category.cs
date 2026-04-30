using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.CategoryModule;

public class Category : BaseEntity<Category>
{
    public string Name { get; internal set; } = string.Empty;
    public string Colour { get; internal set; } = string.Empty;
    public static readonly string[] Categories = ["Nome", "Código da cor"];

    public Category(string name, string colour)
    {
        Name = name;
        Colour = colour;
    }
    public Category(Category c) : this(c.Name, c.Colour) { }

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
