using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Teacher : BaseEntity
    {
        public ICollection<Class> Classes { get; set; }

        public Employee Employee { get; set; }

        public Guid EmployeeID { get; set; }
    }
}
