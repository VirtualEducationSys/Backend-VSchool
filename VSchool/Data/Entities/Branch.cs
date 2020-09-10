using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data.Entities;

namespace VSchool.Data.Entities
{

    public class Branch : BaseEntity
    {
        public string Label { get; set; }
        public string Description { get; set; }

        public Level Level { get; set; }

        public Guid LevelID { get; set; }

        public ICollection<Subject> Subjects { get; set; }

        public ICollection<Class> Classes { get; set; }
    }
}
