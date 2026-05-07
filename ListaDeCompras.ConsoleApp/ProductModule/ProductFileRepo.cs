using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.ProductModule;

public class ProductFileRepo : BaseFileRepository<Product>, IProductRepo
{
    public ProductFileRepo(JsonContext context) : base(context) { }
    protected override Dictionary<Guid, Product> LoadEntities()
    {
        return Context.Products;
    }
}
