using ListaDeCompras.ConsoleApp.CategoryModule;
using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.ProductModule;

public class Product : BaseEntity<Product>
{
    public string Name { get; internal set; } = string.Empty;
    public Category Category;
    public string MeasurementUnit { get; internal set; } = string.Empty;
    public decimal Price;
    public static readonly string[] Categories = ["Nome", "Categoria", "Unidade de medida", "Preço"];

    public Product(string name, Category category, string measurementUnit, decimal price)
    {
        Name = name;
        Category = category;
        MeasurementUnit = measurementUnit;
        Price = price;
    }
    public Product(Product p) : this(p.Name, p.Category, p.MeasurementUnit, p.Price) { }

    public override void UpdateEntity(Product updatedProduct)
    {
        if (!Category.Equals(updatedProduct.Category))
        {
            Category.RemoveProduct(this);
            Category = updatedProduct.Category;
            Category.AddProduct(this);
        }
        Name = updatedProduct.Name;
        MeasurementUnit = updatedProduct.MeasurementUnit;
        Price = updatedProduct.Price;
    }

    public override bool Equals(Product product)
    {
        if (Name != product.Name || Category != product.Category || MeasurementUnit != product.MeasurementUnit || Price != product.Price)
            return false;
        return true;
    }
}
