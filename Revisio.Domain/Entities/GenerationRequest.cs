using Revisio.Domain.Enums;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Revisio.Domain.Entities
{
    public class GenerationRequest
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey(nameof(applicationUser))]
        public string UserId { get; set; }
        public ApplicationUser applicationUser { get; set; }
        public int TotalQuestions { get; set; }
        public ExamMode ExamMode { get; set; }
        public int EssayQuestionNum { get; set; }
        public int TrueFalseQuestionNum { get; set; }
        public int MCQQuestionNum { get; set; }
        public int EasyQuestionNum { get; set; }
        public int MediumQuestionNum { get; set; }
        public int HardQuestionNum { get; set; }
        public GenrateExamStatus GenrateExamStatus { get; set; }
        public ICollection<GenerationRequestLecture> SelectedLectures { get; set; } = new List<GenerationRequestLecture>();
    }
}
