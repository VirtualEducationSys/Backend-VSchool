using System;
using System.Collections.Generic;
using System.Text;

namespace VSchool.Data.Entities
{
    public class Employee : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public Teacher Teacher { get; set; }
    }
}
