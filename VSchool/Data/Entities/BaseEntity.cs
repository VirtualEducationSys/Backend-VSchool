using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    /// <summary>
    /// The base entity for all audited entities.
    /// </summary>
    public abstract class BaseEntity : IBaseEntity
    {
        /// <summary>
        /// Duration
        /// </summary>
        /// <value>
        /// Duration.
        /// </value>
        public int Duration { get; set; }
        
        /// <summary>
        /// Gets ID.
        /// </summary>
        /// <value>
        /// ID guid.
        /// </value>
        public Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the last update timestamp.
        /// </summary>
        /// <value>
        /// The last update timestamp.
        /// </value>
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the Id of last updated user.
        /// </summary>
        /// <value>
        /// The Id of last updated user.
        /// </value>
        public Guid UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets create timestamp.
        /// </summary>
        /// <value>
        /// The create timestamp.
        /// </value>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the Id of create user.
        /// </summary>
        /// <value>
        /// The Id of create user.
        /// </value>
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Row version
        /// </summary>
        public byte[] Timestamp { get; set; }
    }
}
