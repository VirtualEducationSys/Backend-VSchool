using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Material : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int Size { get; set; }
        public string Format { get; set; }
    }
}
