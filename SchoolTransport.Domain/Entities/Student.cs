namespace SchoolTransport.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string Name { get; private set; } = string.Empty;
        public DateTime BirthDate { get; private set; }
        public Guid DriverId { get; private set; }
        public User Driver { get; private set; } = null!;
        public Guid SchoolId { get; private set; }
        public School School { get; private set; } = null!;
        public bool AllowPickupLocationChange { get; private set; }

        protected Student() { }
        public Student(string name, DateTime birthDate, Guid driverId, Guid schoolId, bool allowPickupLocationChange)
        {
            Name = name;
            BirthDate  = birthDate;
            DriverId = driverId;
            SchoolId = schoolId;
            AllowPickupLocationChange = allowPickupLocationChange;
        }

    }
}
