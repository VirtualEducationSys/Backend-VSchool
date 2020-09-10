/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Data.Entities;
using System.Collections.Generic;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing transcoding entities.
    /// </summary>
    public class TranscodingRepository : BaseRepository<RefRxr>, ITranscodingRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranscodingRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public TranscodingRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Saves the RXR items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void SaveRxrItems(IList<RefRxr> items)
        {
            DeleteAll<RefRxr>();
            InsertAll(items);
        }

        /// <summary>
        /// Saves the WMI items.
        /// </summary>
        /// <param name="items">The items.</param>
        public void SaveWmiItems(IList<RefWmi> items)
        {
            DeleteAll<RefWmi>();
            InsertAll(items);
        }
    }
}
