using ADNMutante.Dominio.Context;
using ADNMutante.Dominio.Contracts;

namespace ADNMutante.Dominio.Implementation
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        protected readonly ADNMutanteDbContext DbContext;
        public Repository(ADNMutanteDbContext _dbContext)
        {
            DbContext = _dbContext;
        }

        public virtual void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public async void SaveChangesAsync()
        {
            await DbContext.SaveChangesAsync();
        }
    }
}
