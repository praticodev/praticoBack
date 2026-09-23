using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class RepositoryN<TEntity> : IRepositoryN<TEntity> where TEntity : class, new()
    {
        protected readonly PraticoContext Db;
        protected readonly DbSet<TEntity> DbSet;

        public RepositoryN(PraticoContext db)
        {
            Db = db;
            DbSet = db.Set<TEntity>();
        }
        public virtual async Task<TEntity> Adicionar(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
            return entity;
        }

        public virtual async Task Atualizar(TEntity entity)
        {
            DbSet.Update(entity);
            await SaveChanges();
        }

        public virtual async Task RemoverTodos(IEnumerable<TEntity> colecao)
        {
            DbSet.RemoveRange(colecao);
            await SaveChanges();
        }

        public async Task<int> SaveChanges()
        {
            return await Db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task RemoverTenant(TEntity entity)
        {
            DbSet.Remove(entity);
            await SaveChanges();
        }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}
