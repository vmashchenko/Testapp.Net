namespace TestApp.Domain
{
    public interface IEntityBase { }
    public interface IEntityBase<T> : IEntityBase
    {
        T Id { get; set; }
    }

    public class EntityBase<T>
    {
        public virtual T Id { get; set; }
    }
}
