/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using CarInventory.Data.Entities;
using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This abstract class is a base for all service implementations that access database persistence.
    /// </summary>
    public abstract class BaseRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        /// <summary>
        /// The database.
        /// </summary>
        protected readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected BaseRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Gets the repository for given type of items.
        /// </summary>
        /// <typeparam name="TEntity">The type of items.</typeparam>
        /// <returns>The repository for given type of items.</returns>
        public DbSet<TEntity> Set<TEntity>()
            where TEntity : class, IEntity, new()
        {
            return _db.Set<TEntity>();
        }

        /// <summary>
        /// Gets the Queryable for entities.
        /// </summary>
        /// <returns>
        /// The Queryable for entities.
        /// </returns>
        public IQueryable<T> Query()
        {
            return _db.Set<T>();
        }

        /// <summary>
        /// Gets list of all items for the given type.
        /// </summary>
        /// <typeparam name="TEntity">The type of items.</typeparam>
        /// <returns>List of all items for the given type.</returns>
        protected IList<TEntity> List<TEntity>()
            where TEntity : class, new()
        {
            return _db.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Save changes to DB.
        /// </summary>
        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        /// <summary>
        /// Inserts all items.
        /// </summary>
        /// <typeparam name="TEntity">Type of the items</typeparam>
        /// <param name="items">The items to insert, or null to insert all.</param>
        protected void InsertAll<TEntity>(IList<TEntity> items = null)
            where TEntity : class, new()
        {
            items = items ?? List<TEntity>();
            _db.BulkInsert(items, new BulkConfig { BatchSize = 2000 });
        }

        /// <summary>
        /// Saves (update or inserts) all items.
        /// </summary>
        /// <typeparam name="TEntity">The type of the items.</typeparam>
        /// <param name="items">The items to save, or null to save all.</param>
        protected void SaveAll<TEntity>(IList<TEntity> items = null)
            where TEntity : class, new()
        {
            items = items ?? List<TEntity>();
            _db.BulkInsertOrUpdate(items, new BulkConfig { BatchSize = 2000 });
        }

        /// <summary>
        /// Deletes all items.
        /// </summary>
        /// <typeparam name="TEntity">The type of the items.</typeparam>
        /// <param name="items">The items.</param>
        protected void DeleteAll<TEntity>(IList<TEntity> items = null)
            where TEntity : class, new()
        {
            if (items == null)
            {
                _db.Set<TEntity>().BatchDelete();
            }
            else
            {
                _db.BulkDelete(items, new BulkConfig { BatchSize = 2000 });
            }
        }
    }
}
