using CarInventory.Data.Entities;
using CarInventory.Models;

namespace CarInventory.Data.Repositories
{
    /// <summary>
    /// The Center repository.
    /// </summary>
    public interface ICenterRepository : IGenericRepository<RefCenter, CenterSearchCriteria>
    {
        /// <summary>
        /// Gets center by specified PGEO.
        /// </summary>
        /// <param name="pgeo">The PGEO.</param>
        /// <returns>The Center.</returns>
        RefCenter Get(string pgeo);
    }
}
