namespace Revisio.Application.Common.Models
{
    public class UploadLectureRequest
    {
        public string User_Id { get; set; }
        public string Lecture_Id { get; set; }
        public string Course_Id { get; set; }
        public string Content { get; set; }
    }
}
