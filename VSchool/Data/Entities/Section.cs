using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Section : BaseEntity
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public Section ParentSection {get;set;}
        public Guid? ParentSectionID { get; set; }
        public Course Course { get; set; }
        public Guid CourseID { get; set; }

        public ICollection<Section> Sections { get; set; }
    }
}
