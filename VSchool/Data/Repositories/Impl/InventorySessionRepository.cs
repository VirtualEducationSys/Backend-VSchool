/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Config;
using CarInventory.Data.Entities;
using CarInventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing InventorySession entities.
    /// </summary>
    public class InventorySessionRepository : BaseGenericRepository<InventorySession, InventorySessionSearchCriteria>, IInventorySessionRepository
    {
        /// <summary>
        /// The CC configuration.
        /// </summary>
        private readonly IOptions<CCConfig> _ccConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventorySessionRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="ccConfig">The CC configuration.</param>
        public InventorySessionRepository(AppDbContext dbContext, IOptions<CCConfig> ccConfig) : base(dbContext)
        {
            _ccConfig = ccConfig;
        }

        /// <summary>
        /// Uploads the inventory records.
        /// </summary>
        /// <typeparam name="T">Type of the records.</typeparam>
        /// <param name="sessionId">The session Id.</param>
        /// <param name="items">The items.</param>
        public void UploadInventoryRecords<T>(long sessionId, IList<T> items)
            where T : class, ICentralITFile, new()
        {
            var existing = _db.Set<T>().Where(x => x.IdSession == sessionId).ToList();
            DeleteAll(existing);
            InsertAll(items);
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected override IQueryable<InventorySession> ConstructQueryConditions(
            IQueryable<InventorySession> query, InventorySessionSearchCriteria criteria)
        {
            if (criteria.Status != null)
            {
                query = query.Where(x => x.Status == criteria.Status);
            }

            if (criteria.GeoPoleId != null)
            {
                query = query.Where(x => x.Centers.Any(y => y.Center.Country.IdGeoPole == criteria.GeoPoleId));
            }

            if (criteria.PGEO != null)
            {
                query = query.Where(x => x.Centers.Any(y => y.PGEO == criteria.PGEO));
            }

            if (criteria.ExcludedStatus != null)
            {
                query = query.Where(x => x.Status != criteria.ExcludedStatus);
            }

            if (criteria.Name != null && criteria.Name.Trim().Length > 0)
            {
                string name = criteria.Name.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(name));
            }

            return query;
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<InventorySession> IncludeNavigationProperties(IQueryable<InventorySession> query)
        {
            return query
                .Include(x => x.Centers)
                .ThenInclude(x => x.Center.Country);
        }

        /// <summary>
        /// Updates the child entities by loading them from the database context.
        /// </summary>
        ///
        /// <remarks>
        /// All thrown exceptions will be propagated to caller method.
        /// </remarks>
        ///
        /// <param name="entity">The entity to resolve.</param>
        protected override void ResolveChildEntities(InventorySession entity)
        {
            ResolveEntities(entity.Centers);
        }

        /// <summary>
        /// Updates the <paramref name="existing"/> entity according to <paramref name="newEntity"/> entity.
        /// </summary>
        /// <remarks>Override in child services to update navigation properties.</remarks>
        /// <param name="existing">The existing entity.</param>
        /// <param name="newEntity">The new entity.</param>
        protected override void UpdateEntityFields(InventorySession existing, InventorySession newEntity)
        {
            existing.Centers = newEntity.Centers;
            base.UpdateEntityFields(existing, newEntity);
        }
    }
}
