using System;
using System.Collections.Generic;
using System.Text;
using VSchool.Data.Entities;

namespace VSchool.Data.Entities
{
    public class Level : BaseEntity
    {
        public string Label { get;  set; }
        public string Description { get;  set; }
        public ICollection<Branch> Branchs { get;  set; }
    }
}
