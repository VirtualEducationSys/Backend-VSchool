/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Common;
using CarInventory.Data.Entities;
using CarInventory.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing transcoding entities.
    /// </summary>
    public class ReportRepository : BaseRepository<RefReportSql>, IReportRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReportRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ReportRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        /// <summary>
        /// Gets SQL Report by Id.
        /// </summary>
        /// <param name="id">The SQL Report Id.</param>
        /// <returns></returns>
        public RefReportSql Get(long id)
        {
            var report = _db.Set<RefReportSql>().Find(id);
            Util.CheckFoundEntity(report, id, "id");
            return report;
        }

        /// <summary>
        /// Gets all SQL reports.
        /// </summary>
        /// <returns>
        /// SQL reports.
        /// </returns>
        public IList<RefReportSql> GetAll()
        {
            return _db.Set<RefReportSql>()
                .OrderBy(x => x.ReportCategoryNav.CategoryOrder)
                .ThenBy(x => x.ReportOrder)
                .ToList();
        }

        /// <summary>
        /// Executes the SQL Report query.
        /// </summary>
        /// <param name="reportId">The SQL Report Id.</param>
        /// <param name="criteria">The search criteria.</param>
        /// <returns>
        /// The query execution result.
        /// </returns>
        public ReportQueryResult ExecuteQuery(long reportId, ReportSearchCriteria criteria)
        {
            var report = _db.Set<RefReportSql>().Find(reportId);
            Util.CheckFoundEntity(report, reportId, "reportId");

            var result = new ReportQueryResult();
            using (var connection = _db.Database.GetDbConnection())
            {
                _db.Database.OpenConnection();

                string query = report.SQLRequest;

                // set Session Id parameter
                if (criteria.InventorySessionId != null)
                {
                    query = query.Replace("@InventorySessionId", criteria.InventorySessionId.ToString());
                }

                // get total count
                // result.TotalCount = GetTotalCount(connection, query);

                // read Items
                using (var command = connection.CreateCommand())
                {
                    command.CommandTimeout = 300;

                    /* construct order by
                    var orderBy = !string.IsNullOrWhiteSpace(criteria.SortBy)
                        ? $"{criteria.SortBy} {criteria.SortType}"
                        : "(SELECT NULL)"; */

                    // apply paging & sort, if provided
                    if (criteria.PageSize > 0 && criteria.PageIndex > 0)
                    {
                        int offset = criteria.PageSize * (criteria.PageIndex - 1);
                        query = $"select top 5000 * from ({query}) as tmp ";
                    } 

                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        // read column names
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            result.Headers.Add(reader.GetName(i));
                        }

                        // read all records
                        while (reader.Read())
                        {
                            var record = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object value = reader.GetValue(i);
                                record.Add(value);
                            }

                            result.Items.Add(record);
                        }
                    }
                }
            }

            result.TotalCount = result.Items.Count;
            return result;
        }

        /// <summary>
        /// Gets the total count of the SQL query execution.
        /// </summary>
        /// <param name="connection">The DB connection.</param>
        /// <param name="query">The SQL query.</param>
        /// <returns>Total count.</returns>
        private int GetTotalCount(DbConnection connection, string query)
        {
            using (var command = connection.CreateCommand())
            {
                _db.Database.OpenConnection();

                command.CommandText = $"select count(*) from ({query}) as tmp";
                command.CommandTimeout = 300;
                var scalar = command.ExecuteScalar();
                int count = Convert.ToInt32(scalar);
                return count;
            }
        }
    }
}
