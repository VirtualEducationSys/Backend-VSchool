/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Common;
using CarInventory.Data.Entities;
using CarInventory.Exceptions;
using CarInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This abstract class is a base for all <see cref="IGenericRepository{T,S}" /> service implementations. It
    /// provides CRUD, and search functionality.
    /// </summary>
    /// <typeparam name="T">The type of the managed entities.</typeparam>
    /// <typeparam name="S">The type of the entities search criteria.</typeparam>
    public abstract class BaseGenericRepository<T, S> : BaseRepository<T>, IGenericRepository<T, S>
        where T : class, IEntity
        where S : BaseSearchCriteria
    {
        /// <summary>
        /// The cached name of the entity type.
        /// </summary>
        protected readonly string _entityName = typeof(T).Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseGenericRepository{T,S}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        protected BaseGenericRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Creates given entity.
        /// </summary>
        ///
        /// <param name="entity">The entity to create.</param>
        /// <returns>The created entity.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="entity"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="entity"/> is invalid.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        public T Create(T entity)
        {
            Util.ValidateArgumentNotNull(entity, nameof(entity));

            // get existing child entities from DB, otherwise new entities will be created in database
            ResolveChildEntities(entity);
            var newEntity = _db.Set<T>().Add(entity).Entity;
            SaveChanges();

            return newEntity;
        }

        /// <summary>
        /// Updates given entity.
        /// </summary>
        ///
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        ///
        /// <exception cref="ArgumentNullException">
        /// If <paramref name="entity"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <paramref name="entity"/> is invalid.
        /// </exception>
        /// <exception cref="EntityNotFoundException">
        /// If entity with the given Id doesn't exist in DB.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        public void Update(T entity)
        {
            Util.ValidateArgumentNotNull(entity, nameof(entity));

            T existing = Get(entity.Id, "entity.Id", full: false);

            // get existing child entities from DB, otherwise new entities will be created in database
            ResolveChildEntities(entity);

            // copy fields to existing entity (attach approach doesn't work for child entities)
            UpdateEntityFields(existing, entity);

            SaveChanges();
        }

        /// <summary>
        /// Retrieves entity with the given Id.
        /// </summary>
        ///
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <returns>The retrieved entity.</returns>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="id"/> is not positive.
        /// </exception>
        /// <exception cref="EntityNotFoundException">
        /// If entity with the given Id doesn't exist in DB.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        public T Get(object id)
        {
            return Get(id, full: true);
        }

        /// <summary>
        /// Retrieves entity with the given Id without navigational properties.
        /// </summary>
        ///
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <returns>The retrieved entity.</returns>
        public T GetShallow(object id)
        {
            return Get(id, full: false);
        }

        /// <summary>
        /// Retrieves list of all entities.
        /// </summary>
        ///
        /// <returns>The list of all entities.</returns>
        ///
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        public IList<T> GetAll()
        {
            return _db.Set<T>().ToList();
        }

        /// <summary>
        /// Deletes entity with the given Id.
        /// </summary>
        ///
        /// <param name="id">The id of the entity to delete.</param>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="id"/> is not positive.
        /// </exception>
        /// <exception cref="EntityNotFoundException">
        /// If entity with the given Id doesn't exist in DB.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        public void Delete(object id)
        {
            Util.ValidateArgumentNotNull(id, nameof(id));

            T entity = Get(id, full: true);

            DeleteAssociatedEntities(entity);
            _db.Set<T>().Remove(entity);
            SaveChanges();
        }

        /// <summary>
        /// Retrieves entities matching given search criteria.
        /// </summary>
        ///
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The matched entities.</returns>
        ///
        /// <exception cref="ArgumentNullException">If the <paramref name="criteria"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If the <paramref name="criteria"/> is incorrect,
        /// e.g. PageNumber is negative, or PageNumber is positive and PageSize is not positive.</exception>
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        public SearchResult<T> Search(S criteria)
        {
            Util.CheckSearchCriteria(criteria);
            IQueryable<T> query = IncludeSearchItemNavigationProperties(_db.Set<T>());

            // construct query conditions
            query = ConstructQueryConditions(query, criteria);

            // set total count of filtered records
            var result = new SearchResult<T>
            {
                TotalCount = query.Count()
            };

            // adjust SortBy (e.g. navigation properties should be handled by child services)
            var sortBy = ResolveSortBy(criteria.SortBy);

            if (sortBy != null && sortBy.Contains(","))
            {
                var fields = sortBy.Split(",");
                foreach (string softByField in fields)
                {
                    criteria.SortBy = softByField;

                    // construct SortBy property selector expression
                    query = AddSortingCondition(query, criteria);
                }
            }
            else
            {
                criteria.SortBy = sortBy;

                // construct SortBy property selector expression
                query = AddSortingCondition(query, criteria);
            }

            // select required page
            if (criteria.PageIndex > 0)
            {
                query = query.Skip(criteria.PageSize * (criteria.PageIndex - 1)).Take(criteria.PageSize);
            }

            // execute query and set result properties
            result.Items = query.ToList();
            return result;
        }

        /// <summary>
        /// Retrieves count of entities matching given search criteria.
        /// </summary>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>
        /// The count of matched entities.
        /// </returns>
        public int GetCount(S criteria)
        {
            var query = _db.Set<T>();
            var filteredQuery = ConstructQueryConditions(query, criteria);
            return filteredQuery.Count();
        }

        /// <summary>
        /// Gets the Queryable for entities with included navigation properties.
        /// </summary>
        /// <returns>
        /// The Queryable for entities with included navigation properties.
        /// </returns>
        public IQueryable<T> QueryFull()
        {
            var query = _db.Set<T>();
            var fullQuery = IncludeNavigationProperties(query);
            return fullQuery;
        }

        /// <summary>
        /// Applies filters to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied filters.</returns>
        protected virtual IQueryable<T> ConstructQueryConditions(IQueryable<T> query, S criteria)
        {
            return query;
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
        protected virtual void DeleteAssociatedEntities(T entity)
        {
            // do nothing by default
        }

        /// <summary>
        /// Deletes the child entity from database, if it is not null.
        /// </summary>
        /// <remarks>All exceptions will be propagated to the caller.</remarks>
        /// <typeparam name="TChild">The type of the child.</typeparam>
        /// <param name="entity">The entity to delete.</param>
        protected void DeleteChildEntity<TChild>(TChild entity)
            where TChild : class, IEntity
        {
            if (entity != null)
            {
                _db.Set<TChild>().Remove(entity);
            }
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
        protected virtual void ResolveChildEntities(T entity)
        {
            // do nothing by default
        }

        /// <summary>
        /// Resolves given identifiable entities.
        /// </summary>
        /// <typeparam name="TEntity">The actual type of entities in the collection.</typeparam>
        /// <param name="items">The entities to resolve.</param>
        /// <remarks>All exceptions will be propagated.</remarks>
        protected void ResolveEntities<TEntity>(IList<TEntity> items)
            where TEntity : class, IEntity
        {
            if (items != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    items[i] = ResolveChildEntity(items[i]);
                }
            }
        }

        /// <summary>
        /// Check that entity is not <c>null</c> and tries to retrieve its updated value from the database context.
        /// </summary>
        ///
        /// <remarks>
        /// All thrown exceptions will be propagated to caller method.
        /// </remarks>
        ///
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="id">The Id of the entity.</param>
        /// <param name="isRequired">Specifies whether NotFound exception should be thrown if not found.</param>
        /// <returns>The updated entity from the database context.</returns>
        protected TEntity ResolveChildEntity<TEntity>(TEntity entity, object id = null, bool isRequired = false)
            where TEntity : class, IEntity
        {
            object entityId = id ?? entity?.Id;
            if (entityId == null)
            {
                if (isRequired)
                {
                    throw new ArgumentException(
                        $"Valid Id of {typeof(TEntity).Name} must be provided.");
                }

                return null;
            }

            TEntity child = _db.Set<TEntity>().Find(entityId);
            if (child == null)
            {
                throw new EntityNotFoundException(
                    $"Child entity {typeof(TEntity).Name} with Id={entityId} was not found.");
            }

            return child;
        }

        /// <summary>
        /// Updates the <paramref name="existing"/> entity according to <paramref name="newEntity"/> entity.
        /// </summary>
        /// <remarks>Override in child services to update navigation properties.</remarks>
        /// <param name="existing">The existing entity.</param>
        /// <param name="newEntity">The new entity.</param>
        protected virtual void UpdateEntityFields(T existing, T newEntity)
        {
            _db.Entry(existing).CurrentValues.SetValues(newEntity);
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected virtual IQueryable<T> IncludeNavigationProperties(IQueryable<T> query)
        {
            // by default do not include any child property
            return query;
        }

        /// <summary>
        /// Includes the navigation properties loading for the entity during Search operation.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns>The updated query.</returns>
        protected virtual IQueryable<T> IncludeSearchItemNavigationProperties(IQueryable<T> query)
        {
            // by default do not include any child property
            return query;
        }

        /// <summary>
        /// Gets the resolved SortBy property.
        /// </summary>
        /// <param name="sortBy">The SortBy property value.</param>
        /// <returns>Resolved SortBy property.</returns>
        protected virtual string ResolveSortBy(string sortBy)
        {
            // Note: override in child classes to handle sorting by navigation properties
            if (sortBy == null)
            {
                return "Id";
            }

            return sortBy;
        }

        /// <summary>
        /// Retrieves entity with the given Id.
        /// </summary>
        ///
        /// <param name="id">The id of the entity to retrieve.</param>
        /// <param name="idParamName">The name of the Id parameter in the method.</param>
        /// <param name="full">Determines whether navigation properties should also be loaded.</param>
        /// <returns>The retrieved entity.</returns>
        ///
        /// <exception cref="ArgumentException">
        /// If <paramref name="id"/> is not positive.
        /// </exception>
        /// <exception cref="EntityNotFoundException">
        /// If entity with the given Id doesn't exist in DB.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// If a DB-based error occurs.
        /// </exception>
        /// <exception cref="ServiceException">
        /// If any other errors occur while performing this operation.
        /// </exception>
        protected T Get(object id, string idParamName = "id", bool full = true)
        {
            Util.ValidateArgumentNotNull(id, idParamName);

            IQueryable<T> query = _db.Set<T>();
            if (full)
            {
                query = IncludeNavigationProperties(query);
            }

            T entity = GetById(query, id);
            Util.CheckFoundEntity(entity, id);
            if (full)
            {
                PopulateAdditionalFields(entity);
            }

            return entity;
        }

        /// <summary>
        /// Gets item by Id.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="id">The item Id.</param>
        /// <returns>The item.</returns>
        protected virtual T GetById(IQueryable<T> query, object id)
        {
            return query.FirstOrDefault(e => Equals(e.Id, id));
        }

        /// <summary>
        /// Populates the additional fields.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected virtual void PopulateAdditionalFields(T entity)
        {
            // do nothing
        }

        /// <summary>
        /// Applies ordering to the given query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>The updated query with applied ordering.</returns>
        protected virtual IQueryable<T> AddSortingCondition(IQueryable<T> query, S criteria)
        {
            return criteria.SortType == SortType.ASC
                ? query.OrderBy(criteria.SortBy)
                : query.OrderByDescending(criteria.SortBy);
        }
    }
}
