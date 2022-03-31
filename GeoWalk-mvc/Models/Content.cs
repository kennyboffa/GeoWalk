using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace GeoWalk_mvc.Models
{
    //public class Content
    //{

    //    public int Id { get; set; }
    //    [Required]
    //    public string Title { get; set; }
    //    public string Info { get; set; }
    //    public string Question { get; set; }
    //    public string Answers { get; set; }
    //    public virtual Location Location { get; set; }

    //}


    //EF CORE - how to map abstract classes into one table

    // In ContentController - return all content (with class type)

    // Kolla Tables i HTML


    public abstract class ContentBase
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual Location Location { get; set; }
        public abstract string Type { get; }

        public abstract string Summarize { get; }
    }

    public class InfoContent : ContentBase
    {
        public string Info { get; set; }
        public override string Type { get; } = "info";

            public override string Summarize => Info;
        }
    public class QuestionAnswer : ContentBase
    {
        public string Question { get; set; }
            public override string Summarize => Question;
            public int ApprovedPoints { get; set; } = 1;

        public int AnswersToShow { get; set; } = 3;

        public virtual IEnumerable<Answer> Answers { get; set; }
        public override string Type { get; } = "questionanswer";
    }
    //public class NyContent : ContentBase
    //{
    //    public string Info { get; set; }
    //    public override string Type { get; } = "ny";
    //}

    public class Answer
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
        public int Points { get; set; } = 0;
        public virtual QuestionAnswer QuestionAnswer { get; set; }
    }


}
