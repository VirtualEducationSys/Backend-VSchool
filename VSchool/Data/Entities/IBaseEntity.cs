using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    /// <summary>
    /// Interface for audited entities.
    /// </summary>
    public interface IBaseEntity
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
        Guid ID { get; set; }

        /// <summary>
        /// Gets or sets the last update timestamp.
        /// </summary>
        /// <value>
        /// The last update timestamp.
        /// </value>
        DateTime UpdatedOn { get; set; }

        /// <summary>
        /// Gets or sets the Id of last updated user.
        /// </summary>
        /// <value>
        /// The Id of last updated user.
        /// </value>
        Guid UpdatedBy { get; set; }

        /// <summary>
        /// Gets or sets create timestamp.
        /// </summary>
        /// <value>
        /// The create timestamp.
        /// </value>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the Id of create user.
        /// </summary>
        /// <value>
        /// The Id of create user.
        /// </value>
        Guid CreatedBy { get; set; }

        /// <summary>
        /// Row version
        /// </summary>
        public byte[] Timestamp { get; set; }
    }
}
