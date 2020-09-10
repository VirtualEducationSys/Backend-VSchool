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
    /// This repository class provides operations for managing Region entities.
    /// </summary>
    public class GeoPoleRepository : BaseGenericRepository<RefGeoPole, GeoPoleSearchCriteria>, IGeoPoleRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoPoleRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GeoPoleRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<RefGeoPole> IncludeNavigationProperties(IQueryable<RefGeoPole> query)
        {
            return query
                .Include(x => x.Countries);
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected override IQueryable<RefGeoPole> ConstructQueryConditions(
            IQueryable<RefGeoPole> query, GeoPoleSearchCriteria criteria)
        {
            if (criteria.Status != null)
            {
                query = query.Where(x => x.Status == criteria.Status);
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
    }
}
