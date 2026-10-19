namespace SchoolTransport.Application.DTOs.Students
{
    public class CreateStudentDto
    {
        public string Name { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public Guid SchoolId { get; set; }
        public bool AllowPickupLocationChange { get; set; }
    }
}
