namespace SchoolTransport.Domain.Entities
{
    public class StudentGuardian : BaseEntity
    {
        public Guid StudentId { get; private set; }
        public Student? Student { get; private set; }

        public Guid GuardianId { get; private set; }
        public Guardian? Guardian { get; private set; }

        protected StudentGuardian() { }

        public StudentGuardian(
            Guid studentId,
            Guid guardianId)
        {
            StudentId = studentId;
            GuardianId = guardianId;
        }
    }
}