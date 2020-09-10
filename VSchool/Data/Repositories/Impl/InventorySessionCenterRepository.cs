/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Data.Entities;
using CarInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing InventorySessionCenter entities.
    /// </summary>
    public class InventorySessionCenterRepository : BaseGenericRepository<InventorySessionCenter, InventorySessionCenterSearchCriteria>, IInventorySessionCenterRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InventorySessionCenterRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public InventorySessionCenterRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Uploads the inventory records.
        /// </summary>
        /// <typeparam name="T">Type of the records.</typeparam>
        /// <param name="sessionCenterId">The session center Id.</param>
        /// <param name="items">The items.</param>
        public void UploadInventoryRecords<T>(long sessionCenterId, IList<T> items)
            where T : class, IInventoryLocalFile, new()
        {
            var sessionCenter = Get(sessionCenterId);
            var existing = _db.Set<T>().Where(x => x.IdSession == sessionCenter.IdSession && x.PGEO == sessionCenter.PGEO).ToList();
            DeleteAll(existing);

            InsertAll(items);
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected override IQueryable<InventorySessionCenter> ConstructQueryConditions(
            IQueryable<InventorySessionCenter> query, InventorySessionCenterSearchCriteria criteria)
        {
            if (criteria.GeoPoleId != null)
            {
                query = query.Where(x => x.Center.Country.IdGeoPole == criteria.GeoPoleId);
            }

            if (criteria.PGEO != null)
            {
                query = query.Where(x => x.PGEO == criteria.PGEO);
            }

            if (criteria.ExcludedStatus != null)
            {
                query = query.Where(x => x.Status != criteria.ExcludedStatus);
            }

            if (criteria.SessionId != null)
            {
                query = query.Where(x => x.IdSession == criteria.SessionId);
            }

            return query;
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<InventorySessionCenter> IncludeNavigationProperties(IQueryable<InventorySessionCenter> query)
        {
            return query
                .Include(x => x.Center.Country)
                .Include(x => x.LocalITUpdateUser)
                .Include(x => x.LocalPhysicalUpdateUser)
                .Include(x => x.CentralStatusUpdateUser);
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity during Search operation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The updated query.
        /// </returns>
        protected override IQueryable<InventorySessionCenter> IncludeSearchItemNavigationProperties(IQueryable<InventorySessionCenter> query)
        {
            return query
                .Include(x => x.Center.Country)
                .Include(x => x.LocalITUpdateUser)
                .Include(x => x.LocalPhysicalUpdateUser)
                .Include(x => x.CentralStatusUpdateUser);
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
        protected override void ResolveChildEntities(InventorySessionCenter entity)
        {
            entity.Center = ResolveChildEntity(entity.Center, entity.PGEO);
            entity.Session = ResolveChildEntity(entity.Session, entity.IdSession);
            entity.LocalIT = ResolveChildEntity(entity.LocalIT, entity.IdLocalIT);
        }

        /// <summary>
        /// Updates the <paramref name="existing"/> entity according to <paramref name="newEntity"/> entity.
        /// </summary>
        /// <remarks>Override in child services to update navigation properties.</remarks>
        /// <param name="existing">The existing entity.</param>
        /// <param name="newEntity">The new entity.</param>
        protected override void UpdateEntityFields(InventorySessionCenter existing, InventorySessionCenter newEntity)
        {
            existing.Center = newEntity.Center;
            existing.Session = newEntity.Session;
            existing.LocalIT = newEntity.LocalIT;
            base.UpdateEntityFields(existing, newEntity);
        }

        /// <summary>
        /// Gets the resolved SortBy property.
        /// </summary>
        /// <param name="sortBy">The SortBy property value.</param>
        /// <returns>
        /// Resolved SortBy property.
        /// </returns>
        protected override string ResolveSortBy(string sortBy)
        {
            // override default behavior which replace null with Id
            return sortBy;
        }

        /// <summary>
        /// Applies ordering to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>
        /// The updated query with applied ordering.
        /// </returns>
        protected override IQueryable<InventorySessionCenter> AddSortingCondition(IQueryable<InventorySessionCenter> query, InventorySessionCenterSearchCriteria criteria)
        {
            if (criteria.SortBy == null)
            {
                return query.OrderBy(x => x.LocalSessionDate).ThenBy(x => x.PGEO);
            }
            else
            {
                return base.AddSortingCondition(query, criteria);
            }
        }
    }
}
