using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data.Entities;

namespace VSchool.Data.Entities
{
    public class Subject : BaseEntity
    {
        public string Label { get; set; }
        public string Description { get; set; }

        public Branch Branch { get; set; }

        public Guid BranchID { get; set; }
    }
}
