﻿/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using Microsoft.EntityFrameworkCore;
using System.Linq;
using VSchool.Data.Entities;

namespace VSchool.Data.Repositories
{
    /// <summary>
    /// This repository interface defines basic methods to manage <typeparamref name="T" /> entities.
    /// </summary>
    /// <typeparam name="T">The type of the managed entities.</typeparam>
    public interface IRepository<T>
        where T : IBaseEntity
    {
        /// <summary>
        /// Saves current changes.
        /// </summary>
        void SaveChanges();

        /// <summary>
        /// Gets the Queryable for entities.
        /// </summary>
        /// <returns>The Queryable for entities.</returns>
        IQueryable<T> Query();

        /// <summary>
        /// Gets DB Set of the given type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>DB Set.</returns>
        DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity, new();
    }
}
