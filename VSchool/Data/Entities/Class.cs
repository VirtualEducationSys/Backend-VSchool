using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Class : BaseEntity
    {
        public string Label { get; set; }
        public Branch Branch { get; set; }

        public Guid BranchID { get; set; }
        public ICollection<Student> Students { get; set; }

        public Teacher Teacher { get; set; }

        public Guid TeacherID { get; set; }

    }
}
