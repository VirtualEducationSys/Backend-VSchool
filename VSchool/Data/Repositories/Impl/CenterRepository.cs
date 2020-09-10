/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Common;
using CarInventory.Data.Entities;
using CarInventory.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing Center entities.
    /// </summary>
    public class CenterRepository : BaseGenericRepository<RefCenter, CenterSearchCriteria>, ICenterRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CenterRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public CenterRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets center by specified PGEO.
        /// </summary>
        /// <param name="pgeo">The PGEO.</param>
        /// <returns>
        /// The Center.
        /// </returns>
        public RefCenter Get(string pgeo)
        {
            Util.ValidateArgumentNotNull(pgeo, nameof(pgeo));

            IQueryable<RefCenter> query = Query();
            query = IncludeNavigationProperties(query);

            RefCenter entity = query.FirstOrDefault(e => e.PGEO == pgeo);
            Util.CheckFoundEntity(entity, pgeo);
            PopulateAdditionalFields(entity);

            return entity;
        }

        /// <summary>
        /// Gets the Center by Id.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="id">The center Id.</param>
        /// <returns></returns>
        protected override RefCenter GetById(IQueryable<RefCenter> query, object id)
        {
            string pgeo = id.ToString();
            return query.FirstOrDefault(x => x.PGEO == pgeo);
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected override IQueryable<RefCenter> ConstructQueryConditions(
            IQueryable<RefCenter> query, CenterSearchCriteria criteria)
        {
            if (criteria.RegionId != null)
            {
                query = query.Where(x => x.Country.IdGeoPole == criteria.RegionId);
            }

            if (criteria.PGEO != null)
            {
                query = query.Where(x => x.PGEO == criteria.PGEO);
            }

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
                query = query.Where(x => x.Name.ToLower().Contains(name) || x.PGEO.ToLower().Contains(name));
            }

            return query;
        }

        /// <summary>
        /// Applies ordering to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>
        /// The updated query with applied ordering.
        /// </returns>
        protected override IQueryable<RefCenter> AddSortingCondition(IQueryable<RefCenter> query, CenterSearchCriteria criteria)
        {
            if ("IsAssociated".IsCaseInsensitiveEqual(criteria.SortBy) && criteria.AssociatedCenters != null)
            {
                return criteria.SortType == SortType.ASC
                    ? query.OrderBy(x => criteria.AssociatedCenters.Contains(x.PGEO) ? 0 : 1).ThenBy(x => x.PGEO)
                    : query.OrderBy(x => criteria.AssociatedCenters.Contains(x.PGEO) ? 1 : 0).ThenBy(x => x.PGEO);
            }

            return base.AddSortingCondition(query, criteria);
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<RefCenter> IncludeNavigationProperties(IQueryable<RefCenter> query)
        {
            return query
                .Include(x => x.Country);
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity during Search operation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The updated query.
        /// </returns>
        protected override IQueryable<RefCenter> IncludeSearchItemNavigationProperties(IQueryable<RefCenter> query)
        {
            return query
                .Include(x => x.Country);
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
        protected override void ResolveChildEntities(RefCenter entity)
        {
            entity.Country = ResolveChildEntity(entity.Country, entity.IdCountry);
        }

        /// <summary>
        /// Updates the <paramref name="existing"/> entity according to <paramref name="newEntity"/> entity.
        /// </summary>
        /// <remarks>Override in child services to update navigation properties.</remarks>
        /// <param name="existing">The existing entity.</param>
        /// <param name="newEntity">The new entity.</param>
        protected override void UpdateEntityFields(RefCenter existing, RefCenter newEntity)
        {
            existing.Country = newEntity.Country;
            base.UpdateEntityFields(existing, newEntity);
        }

        /// <summary>
        /// Deletes the associated entities from the database context.
        /// </summary>
        ///
        /// <remarks>
        /// All thrown exceptions will be propagated to caller method.
        /// </remarks>
        ///
        /// <param name="entity">The entity to delete associated entities for.</param>
        protected override void DeleteAssociatedEntities(RefCenter entity)
        {
        }
    }
}
