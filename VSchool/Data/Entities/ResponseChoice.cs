using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class ResponseChoice : BaseEntity
    {
        public string Choice { get; set; }
        public Question Question { get; set; }
        public Guid QuestionID { get; set; }
    }
}
