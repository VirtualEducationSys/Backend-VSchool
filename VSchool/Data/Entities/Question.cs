using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Question : BaseEntity
    {
        public string ExerciceQuestion { get; set; }
        public Quize Quize { get; set; }
        public Guid QuizeID { get; set; }

        public ICollection<ResponseChoice> Choices { get; set; }
    }
}
