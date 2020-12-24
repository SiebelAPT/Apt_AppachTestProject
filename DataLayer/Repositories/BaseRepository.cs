using Core;
using Core.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLayer
{
    /// <summary>
    /// <inheritdoc cref="TRepository"/>
    /// </summary>
    /// <typeparam name="TIdentity"><see cref="TIdentity"/></typeparam>
    /// <typeparam name="TEntity"><see cref="TEntity"/></typeparam>
    /// <typeparam name="TRepository"><see cref="TRepository"/></typeparam>
    public abstract class BaseRepository<TIdentity, TEntity, TRepository> : IBaseRepository<TIdentity, TEntity, TRepository>
        where TEntity : class
        where TRepository : BaseRepository<TIdentity, TEntity, TRepository>
    {
        #region Define Variables

        protected readonly DbContext Context;
        protected IQueryable<TEntity> Query;

        #endregion

        #region Constructor

        protected BaseRepository(DbContext context)
        {
            Context = context;
            Query = context.Set<TEntity>();
        }

        #endregion

        #region Public Methods

        public virtual async Task<TEntity> ByIdAsync(TIdentity id, CancellationToken cancellationToken = default)
        {
            var result = await Context.FindAsync<TEntity>(new object[] { id }, cancellationToken);
            if (result == null) throw new NotFoundException<TIdentity>(typeof(TEntity).Name, id);
            return result;
        }

        public async Task<TEntity> ToSingleAsync(CancellationToken cancellationToken = default)
        {
            return await Query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<TEntity>> ToListAsync(CancellationToken cancellationToken = default)
        {
            return await
                Query.ToListAsync(cancellationToken);
        }

        public TRepository NoTrack()
        {
            Query.AsNoTracking();
            return this as TRepository;
        }

        public virtual async Task<TEntity> CreateRecordAsync(TEntity record, CancellationToken cancellationToken = default)
        {
            var result = await Context.AddAsync(record, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
            return result.Entity;

        }

        public virtual async Task<int> UpdateRecordAsync(TEntity record, CancellationToken cancellationToken = default)
        {
            var result = Context.Attach(record);
            result.State = EntityState.Modified;
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity record, CancellationToken cancellationToken = default)
        {
            var result = Context.Update(record);
            result.State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken);
            return result.Entity;
        }

        public virtual async Task<int> DeleteRecordAsync(TIdentity id, CancellationToken cancellationToken = default)
        {
            var item = await Context.FindAsync<TEntity>(id);
            if (item == null) throw new NotFoundException<TIdentity>(typeof(TEntity).Name, id);
            Context.Remove(item);
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<int> PatchRecordAsync(TIdentity id, string data, CancellationToken cancellationToken = default)
        {
            var item = await Context.FindAsync<TEntity>(id);
            if (item == null) throw new NotFoundException<TIdentity>(typeof(TEntity).Name, id);
            JsonConvert.PopulateObject(data, item);
            Context.Entry(item).State = EntityState.Modified;
            return await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> CreateBulkAsync(IEnumerable<TEntity> records, CancellationToken cancellationToken = default)
        {
            var bulkAsync = records.ToList();
            foreach (var record in bulkAsync)
            {
                await Context.AddAsync(record, cancellationToken);
            }

            await Context.SaveChangesAsync(cancellationToken);
            return bulkAsync;
        }

        public virtual async Task<int> DeleteBulkAsync(IEnumerable<TIdentity> ids, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                var item = await Context.FindAsync<TEntity>(id);
                if (item == null) throw new NotFoundException<TIdentity>(typeof(TEntity).Name, id);
                Context.Remove(item);
            }

            return await Context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(TIdentity id, CancellationToken cancellationToken = default)
        {
            return (await Context.FindAsync<TEntity>(id)) != null;
        }

        public TRepository Reset()
        {
            Query = Context.Set<TEntity>().AsQueryable();
            return this as TRepository;
        }

        public virtual async Task<TEntity> SelectFirstOrDefaultAsync(CancellationToken cancellationToken = default)
        {
            return await Query.FirstOrDefaultAsync(cancellationToken);
        }            

        #endregion
    }
}
