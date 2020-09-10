/*
 * Copyright (c) 2019, TopCoder, Inc. All rights reserved.
 */
using CarInventory.Data.Entities;
using CarInventory.Models;
using System.Collections.Generic;
using System.Linq;

namespace CarInventory.Data.Repositories.Impl
{
    /// <summary>
    /// This repository class provides operations for managing translated text.
    /// </summary>
    public class TranslatedTextRepository : BaseRepository<TranslatedText>, ITranslatedTextRepository
    {
        /// <summary>
        /// The property name mapper.
        /// </summary>
        private readonly INameMapper _nameMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatedTextRepository" /> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="nameMapper">The name mapper.</param>
        public TranslatedTextRepository(AppDbContext dbContext, INameMapper nameMapper) : base(dbContext)
        {
            _nameMapper = nameMapper;
        }

        /// <summary>
        /// Gets the default text.
        /// </summary>
        /// <returns>
        /// The default text.
        /// </returns>
        public IList<CorporateCodeModel> GetDefaultText()
        {
            var result = new List<CorporateCodeModel>
            {
                GetCorporateCodeValues<CCCenterStatus>(),
                GetCorporateCodeValues<CCCenterType>(),
                GetCorporateCodeValues<CCInventoryFileType>(),
                GetCorporateCodeValues<CCInventoryMissingType>(),
                GetCorporateCodeValues<CCLocalIT>(),
                GetCorporateCodeValues<CCLocalITFileStatus>(),
                GetCorporateCodeValues<CCPhysicalFileStatus>(),
                GetCorporateCodeValues<CCRecordStatus>(),
                GetCorporateCodeValues<CCReportCategory>(),
                GetCorporateCodeValues<CCSessionStatus>(),
                GetCorporateCodeValues<CCUserRole>()
            };

            return result;
        }

        /// <summary>
        /// Gets the translated text for a given language.
        /// </summary>
        /// <param name="language">The language.</param>
        /// <param name="languageVariant">The language variant.</param>
        /// <returns>
        /// The translated text.
        /// </returns>
        public IList<CorporateCodeModel> GetTranslatedText(string language, string languageVariant)
        {
            var items = Query()
                .Where(x => x.Language == language && (languageVariant == null || x.LanguageVariant == languageVariant))
                .ToList();

            var result = new List<CorporateCodeModel>();
            foreach (var group in items.GroupBy(x => x.TableName))
            {
                var model = new CorporateCodeModel
                {
                    CorporateCode = group.Key,
                    Items = new List<CorporateCodeItemModel>()
                };

                foreach (var item in group)
                {
                    if (int.TryParse(item.PKColumnValue, out int code))
                    {
                        model.Items.Add(new CorporateCodeItemModel
                        {
                            Code = code,
                            Value = item.TranslatedColumnValue
                        });
                    }
                }

                result.Add(model);
            }

            return result;
        }

        /// <summary>
        /// Gets the corporate code values.
        /// </summary>
        /// <typeparam name="T">Type of records.</typeparam>
        /// <returns>Corporate code values.</returns>
        private CorporateCodeModel GetCorporateCodeValues<T>()
            where T : BaseCCEntity, new()
        {
            return new CorporateCodeModel
            {
                CorporateCode = _nameMapper.GetTableName<T>(),
                Items = Set<T>().Select(x => new CorporateCodeItemModel
                {
                    Code = x.Id,
                    Value = x.Value
                }).ToList()
            };
        }
    }
}
