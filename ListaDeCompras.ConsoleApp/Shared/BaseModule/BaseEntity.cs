namespace ListaDeCompras.ConsoleApp.Shared.BaseModule;

public abstract class BaseEntity<T> where T : BaseEntity<T>
{
    public Guid Id { get; set; }
    public abstract void UpdateEntity(T updatedEntity);
    public abstract bool Equals(T entity);
}
