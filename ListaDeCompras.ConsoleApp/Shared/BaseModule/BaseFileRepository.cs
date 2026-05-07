namespace ListaDeCompras.ConsoleApp.Shared.BaseModule;

public abstract class BaseFileRepository<T> : BaseRepository<T> where T : BaseEntity<T>
{
    protected readonly JsonContext Context;
    public BaseFileRepository(JsonContext context)
    {
        Context = context;
        Entities = LoadEntities();
    }

    protected abstract Dictionary<Guid, T> LoadEntities();

    public override void Add(T entity)
    {
        base.Add(entity);
        Context.Save();
    }

    public override bool Edit(Guid id, T updatedEntity)
    {
        bool baseResult = base.Edit(id, updatedEntity);
        if (baseResult) Context.Save();
        return baseResult;
    }

    public override bool Remove(Guid id)
    {
        bool baseResult = base.Remove(id);
        if (baseResult) Context.Save();
        return baseResult;
    }
}
