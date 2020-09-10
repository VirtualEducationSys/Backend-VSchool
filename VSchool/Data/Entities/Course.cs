using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data.Entities;

namespace VSchool.Data.Entities
{
    public class Course : BaseEntity
    {
        public string Label { get; set; }
        public string Description { get; set; }

        public Guid SubjectID { get; set; }
        public Subject Subject { get; set; }

        public ICollection<Section> Sections { get; set; }
    }
}
