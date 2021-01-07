namespace ADNMutante.Dominio.Contracts
{
    public interface IRepository <TEntity> where TEntity : class, new()
    {
        void Add(TEntity entity);
        void SaveChanges();
        void SaveChangesAsync();
    }
}
