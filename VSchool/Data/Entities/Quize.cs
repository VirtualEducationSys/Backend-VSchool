using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Quize : BaseEntity
    {
        public string Case { get; set; }

        public int Type { get; set; }
        public ICollection<Question> Questions { get; set; }
    }
}
