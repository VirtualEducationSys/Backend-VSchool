/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Data.Entities;
using CarInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing Country entities.
    /// </summary>
    public class CountryRepository : BaseGenericRepository<RefCountry, CountrySearchCriteria>, ICountryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CountryRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CountryRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected override IQueryable<RefCountry> ConstructQueryConditions(
            IQueryable<RefCountry> query, CountrySearchCriteria criteria)
        {
            if (criteria.Status != null)
            {
                query = query.Where(x => x.Status == criteria.Status);
            }

            if (criteria.GeoPoleId != null)
            {
                query = query.Where(x => x.IdGeoPole == criteria.GeoPoleId);
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
        protected override IQueryable<RefCountry> IncludeNavigationProperties(IQueryable<RefCountry> query)
        {
            return query
                .Include(x => x.GeoPole);
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity during Search operation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The updated query.
        /// </returns>
        protected override IQueryable<RefCountry> IncludeSearchItemNavigationProperties(IQueryable<RefCountry> query)
        {
            return query
                .Include(x => x.GeoPole);
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
        protected override void ResolveChildEntities(RefCountry entity)
        {
            entity.GeoPole = ResolveChildEntity(entity.GeoPole, entity.IdGeoPole);
        }

        /// <summary>
        /// Updates the <paramref name="existing"/> entity according to <paramref name="newEntity"/> entity.
        /// </summary>
        /// <remarks>Override in child services to update navigation properties.</remarks>
        /// <param name="existing">The existing entity.</param>
        /// <param name="newEntity">The new entity.</param>
        protected override void UpdateEntityFields(RefCountry existing, RefCountry newEntity)
        {
            existing.GeoPole = newEntity.GeoPole;

            base.UpdateEntityFields(existing, newEntity);
        }
    }
}
