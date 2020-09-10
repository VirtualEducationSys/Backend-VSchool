using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSchool.Data.Entities
{
    public class Response : BaseEntity
    {
        public Student Student { get; set; }

        public Guid StudentID { get; set; }
        public Question Question { get; set; }
        public Guid QuestionID { get; set; }

        public double Score { get; set; }

        public int Accurancy { get; set; }
        public Guid GivenByID { get; set; }

        public string Awnser {get;set;}
        public Teacher GivenBy { get; set; }
        public ICollection<ResponseChoice> SelectedChoices { get; set; }

        public Guid MaterialID { get; set; }

    }
}
