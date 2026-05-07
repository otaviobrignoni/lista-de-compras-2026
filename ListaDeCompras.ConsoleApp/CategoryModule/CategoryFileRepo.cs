using ListaDeCompras.ConsoleApp.Shared.BaseModule;

namespace ListaDeCompras.ConsoleApp.CategoryModule;

public class CategoryFileRepo : BaseFileRepository<Category>, ICategoryRepo
{
    public CategoryFileRepo(JsonContext context) : base(context) { }
    protected override Dictionary<Guid, Category> LoadEntities()
    {
        return Context.Categories;
    }
}
