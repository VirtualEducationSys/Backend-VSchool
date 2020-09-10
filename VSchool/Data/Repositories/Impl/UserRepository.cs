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
    /// This repository class provides operations for managing User entities.
    /// </summary>
    public class UserRepository : BaseGenericRepository<RefUser, UserSearchCriteria>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets user by login.
        /// </summary>
        /// <param name="login">The user login.</param>
        /// <returns>
        /// The found User entity.
        /// </returns>
        public RefUser GetByLogin(string login)
        {
            RefUser user = IncludeNavigationProperties(_db.Users).FirstOrDefault(x => x.Login == login);
            return user;
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected override IQueryable<RefUser> ConstructQueryConditions(
            IQueryable<RefUser> query, UserSearchCriteria criteria)
        {
            if (criteria.Status != null)
            {
                query = query.Where(x => x.Status == criteria.Status);
            }

            if (criteria.PGEO != null)
            {
                query = query.Where(x => x.PGEO == criteria.PGEO);
            }

            if (criteria.RegionId != null)
            {
                query = query.Where(x => x.GeoPoleId == criteria.RegionId);
            }

            if (criteria.ExcludedStatus != null)
            {
                query = query.Where(x => x.Status != criteria.ExcludedStatus);
            }

            if (criteria.Name != null && criteria.Name.Trim().Length > 0)
            {
                string name = criteria.Name.ToLower();
                query = query.Where(x => x.Login.ToLower().Contains(name) ||
                                        (x.FirstName != null && x.FirstName.ToLower().Contains(name)) ||
                                        (x.LastName != null && x.LastName.ToLower().Contains(name)) ||
                                        x.Email.ToLower().Contains(name) ||
                                        (x.Center != null && x.Center.Country != null && x.Center.Country.Name != null
                                            && x.Center.Country.Name.ToLower().Contains(name)));
            }

            return query;
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected override IQueryable<RefUser> IncludeNavigationProperties(IQueryable<RefUser> query)
        {
            return query
                .Include(x => x.Role)
                .Include(x => x.Center.Country)
                .Include(x => x.GeoPole);
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity during Search operation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>
        /// The updated query.
        /// </returns>
        protected override IQueryable<RefUser> IncludeSearchItemNavigationProperties(IQueryable<RefUser> query)
        {
            return query
                .Include(x => x.Role)
                .Include(x => x.Center.Country)
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
        protected override void ResolveChildEntities(RefUser entity)
        {
            entity.Role = ResolveChildEntity(entity.Role, entity.RoleId);
            entity.Center = ResolveChildEntity(entity.Center, entity.PGEO);
            entity.GeoPole = ResolveChildEntity(entity.GeoPole, entity.GeoPoleId);
        }

        /// <summary>
        /// Updates the <paramref name="existing"/> entity according to <paramref name="newEntity"/> entity.
        /// </summary>
        /// <remarks>Override in child services to update navigation properties.</remarks>
        /// <param name="existing">The existing entity.</param>
        /// <param name="newEntity">The new entity.</param>
        protected override void UpdateEntityFields(RefUser existing, RefUser newEntity)
        {
            // prevent from modifying following properties
            newEntity.PasswordHash = existing.PasswordHash;

            existing.Role = newEntity.Role;
            existing.Center = newEntity.Center;
            existing.GeoPole = newEntity.GeoPole;

            base.UpdateEntityFields(existing, newEntity);
        }
    }
}
